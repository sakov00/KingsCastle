using System;
using System.Collections.Generic;
using _Project.Scripts._GlobalLogic;
using _Project.Scripts._VContainer;
using _Project.Scripts.AllAppData;
using _Project.Scripts.Enums;
using _Project.Scripts.Extentions;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Pools;
using _Project.Scripts.Registries;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Splines;
using VContainer;
using ISavableModel = _Project.Scripts.Interfaces.ISavableModel;
using Random = UnityEngine.Random;
using Vector2Scaled = _Project.Scripts.DTO.Vector2Scaled;
using Vector3Scaled = _Project.Scripts.DTO.Vector3Scaled;

namespace _Project.Scripts.GameObjects.Additional.EnemyRoads
{
    [Serializable]
    [RequireComponent(typeof(SplineContainer))]
    public class EnemyRoadController : MonoBehaviour, ISavableController, IDestroyable
    {
        [Inject] private AppData _appData;
        [Inject] private UnitPool _unitPool;
        [Inject] private GameTimer _gameTimer;
        [Inject] private SaveRegistry _saveRegistry;
        
        [SerializeField] private EnemyRoadModel _model;
        [SerializeField] private EnemyRoadView _view;
        [SerializeField] private SplineContainer _splineContainer;
        [SerializeField] private MeshFilter _meshFilter;

        private List<EnemyWithTime> _currentEnemyList;
        public int CountRounds => _model.RoundEnemyList.Count;

        private void OnValidate()
        {
            _splineContainer ??= GetComponent<SplineContainer>();
        }

        private void Awake()
        {
            InjectManager.Inject(this);
        }

        public UniTask InitializeAsync()
        {
            _saveRegistry.Register(this);

            _model.SplineContainerData = _splineContainer.ToData();

            _model.WorldPositions.Clear();
            foreach (var knot in _splineContainer.Spline)
            {
                var worldPosition = _splineContainer.transform.TransformPoint(knot.Position);
                _model.WorldPositions.Add(worldPosition);
            }
            
            StartSpawn(false);

            _view.RefreshInfoRound(_splineContainer, _model.RoundEnemyList);
            CreateRoadMesh();
            return default;
        }

        public ISavableModel GetSavableModel()
        {
            if (_meshFilter != null && _meshFilter.sharedMesh != null)
            {
                var mesh = _meshFilter.sharedMesh;

                var vertices = mesh.vertices;
                _model.Vertices = new Vector3Scaled[vertices.Length];
                for (var i = 0; i < vertices.Length; i++)
                    _model.Vertices[i] = new Vector3Scaled(vertices[i]);

                var normals = mesh.normals;
                _model.Normals = new Vector3Scaled[normals.Length];
                for (var i = 0; i < normals.Length; i++)
                    _model.Normals[i] = new Vector3Scaled(normals[i]);

                var uvs = mesh.uv;
                _model.UVs = new Vector2Scaled[uvs.Length];
                for (var i = 0; i < uvs.Length; i++)
                    _model.UVs[i] = new Vector2Scaled(uvs[i]);

                var tris = mesh.triangles;
                _model.Triangles = new ushort[tris.Length];
                for (var i = 0; i < tris.Length; i++)
                    _model.Triangles[i] = (ushort)tris[i];
            }
            
            _model.SavePosition = transform.position;
            _model.SaveRotation = transform.rotation;
            return _model;
        }

        public void SetSavableModel(ISavableModel savableModel)
        {
            _model.LoadData(savableModel);
            _splineContainer.ApplyData(_model.SplineContainerData);
        }

        private void CreateRoadMesh()
        {
            if (_meshFilter != null && _model.Vertices != null && _model.UVs != null && _model.Triangles != null)
            {
                var vertices = new Vector3[_model.Vertices.Length];
                for (var i = 0; i < vertices.Length; i++)
                    vertices[i] = _model.Vertices[i].ToVector3();

                var uvs = new Vector2[_model.UVs.Length];
                for (var i = 0; i < uvs.Length; i++)
                    uvs[i] = _model.UVs[i].ToVector2();

                var triangles = new ushort[_model.Triangles.Length];
                for (var i = 0; i < triangles.Length; i++)
                    triangles[i] = _model.Triangles[i];

                var normals = new Vector3[_model.Normals.Length];
                for (var i = 0; i < normals.Length; i++)
                    normals[i] = _model.Normals[i].ToVector3();

                _meshFilter.mesh.Clear();
                _meshFilter.mesh.SetVertices(vertices);
                _meshFilter.mesh.SetUVs(0, uvs);
                _meshFilter.mesh.SetTriangles(triangles, 0);
                _meshFilter.mesh.SetNormals(normals);
            }
        }

        public void StartSpawn(bool isNew = true)
        {
            if (_appData.LevelData.IsFighting == false && isNew == false) return;
            
            var currentRound = _appData.LevelData.CurrentRound;
            if (currentRound >= _model.RoundEnemyList.Count)
            {
                Debug.LogWarning("Нет настроек для текущего раунда спавна.");
                return;
            }

            _currentEnemyList = _model.RoundEnemyList[currentRound].enemies;
            _currentEnemyList.Sort((a, b) => a.time.CompareTo(b.time));
            if (isNew)
            {
                _model.CurrentIndex = 0;
                _model.ElapsedTime = 0f;
            }

            _gameTimer.Unsubscribe(Tick);
            _gameTimer.Subscribe(1f, Tick);
        }

        private void Tick()
        {
            _model.ElapsedTime += 1f;

            while (_model.CurrentIndex < _currentEnemyList.Count &&
                   _model.ElapsedTime >= _currentEnemyList[_model.CurrentIndex].time)
            {
                Spawn(_currentEnemyList[_model.CurrentIndex]);
                _model.CurrentIndex++;
            }

            if (_model.CurrentIndex >= _currentEnemyList.Count) _gameTimer.Unsubscribe(Tick);
        }

        private void Spawn(EnemyWithTime enemyData)
        {
            var offsetX = Random.Range(-5f, 5f);
            var wayPoints = new List<Vector3>();
            foreach (var position in _model.WorldPositions) wayPoints.Add(position + new Vector3(offsetX, 0f, 0f));
            var enemyController = _unitPool.Get(enemyData.enemyType, wayPoints[0]);
            enemyController.InitializeAsync();
            enemyController.SetWayToPoint(wayPoints);
        }
        
        public void Destroy() => Destroy(gameObject);
        
        private void OnDestroy()
        {
            _gameTimer.Unsubscribe(Tick);
            _saveRegistry.Unregister(this);
        }
    }
}