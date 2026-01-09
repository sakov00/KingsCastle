using _Project.Scripts._VContainer;
using _Project.Scripts.AllAppData;
using _Project.Scripts.GameObjects.ActionSystems;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Registries;
using Cysharp.Threading.Tasks;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using VContainer;
using ISavableModel = _Project.Scripts.Interfaces.ISavableModel;
using Vector2Scaled = _Project.Scripts.DTO.Vector2Scaled;
using Vector3Scaled = _Project.Scripts.DTO.Vector3Scaled;

namespace _Project.Scripts.GameObjects.Additional.LevelEnvironment.Terrain
{
    public class TerrainController : MonoBehaviour, ISavableController, IDestroyable
    {
        [Inject] private AppData _appData;
        [Inject] private SaveRegistry _saveRegistry;
        
        [SerializeField] private TerrainModel _model;
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private MeshCollider _meshCollider;
        [SerializeField] private NavMeshSurface _meshSurface;
        
        private void Awake()
        {
            InjectManager.Inject(this);
        }
        
        public async UniTask InitializeAsync()
        {
            _saveRegistry.Register(this);
            await ChangeTerrain();
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

        public void SetSavableModel(ISavableModel savableModel) =>
            _model.LoadData(savableModel);

        private async UniTask ChangeTerrain()
        {
            if (_meshFilter != null && _model.Vertices != null && _model.UVs != null && _model.Triangles != null)
            {
                var vertices = new Vector3[_model.Vertices.Length];
                for (var i = 0; i < vertices.Length; i++)
                    vertices[i] = _model.Vertices[i].ToVector3();
                
                await UniTask.Yield();

                var uvs = new Vector2[_model.UVs.Length];
                for (var i = 0; i < uvs.Length; i++)
                    uvs[i] = _model.UVs[i].ToVector2();
                
                await UniTask.Yield();

                var triangles = new ushort[_model.Triangles.Length];
                for (var i = 0; i < triangles.Length; i++)
                    triangles[i] = _model.Triangles[i];
                
                await UniTask.Yield();

                var normals = new Vector3[_model.Normals.Length];
                for (var i = 0; i < normals.Length; i++)
                    normals[i] = _model.Normals[i].ToVector3();
                
                await UniTask.Yield();

                _meshFilter.mesh.Clear();
                _meshFilter.mesh.SetVertices(vertices);
                _meshFilter.mesh.SetUVs(0, uvs);
                _meshFilter.mesh.SetTriangles(triangles, 0);
                _meshFilter.mesh.SetNormals(normals);
            }
            _meshCollider.sharedMesh = null;
            _meshCollider.sharedMesh = _meshFilter.mesh;
            
            await UniTask.Yield();
            _meshSurface.BuildNavMesh();
        }

        private float GetAreaValue()
        {
            var worldSize = Vector3.Scale(_meshFilter.sharedMesh.bounds.size, transform.lossyScale);
            var groundArea = worldSize.x * worldSize.z;
            return groundArea;
        }

        public void Destroy() => Destroy(gameObject);

        private void OnDestroy()
        {
            _saveRegistry.Unregister(this);
        }
    }
}