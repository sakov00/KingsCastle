using System.Collections.Generic;
using _Project.Scripts.AllAppData;
using _Project.Scripts.GameObjects.Abstract.BaseObject;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Registries;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts.GameObjects.ActionSystems
{
    public class SpatialDetectionSystem : ITickable
    {
        [Inject] private AppData _appData;
        [Inject] private LiveRegistry _liveRegistry;
        
        private float _cellSize = 20f;
        private readonly Dictionary<Vector2Int, List<ObjectController>> _grid = new(128);
        
        public void Tick()
        {
            if (_appData.LevelData.IsFighting)
            {
                UpdateGrid();
                DetectAll();
            }
        }

        private Vector2Int GetCell(Vector3 pos)
        {
            return new Vector2Int(
                Mathf.FloorToInt(pos.x / _cellSize),
                Mathf.FloorToInt(pos.z / _cellSize));
        }

        public void UpdateGrid()
        {
            _grid.Clear();
            var all = _liveRegistry.GetAllReactive();
            int count = all.Count;

            for (int i = 0; i < count; i++)
            {
                var obj = all[i];
                if (obj == null || obj.CurrentHealth <= 0)
                    continue;

                Vector3 pos = obj.transform.position;
                Vector2Int cell = GetCell(pos);

                if (!_grid.TryGetValue(cell, out var list))
                {
                    list = new List<ObjectController>(8);
                    _grid[cell] = list;
                }
                list.Add(obj);
            }
        }

        public void DetectAll()
        {
            var all = _liveRegistry.GetAllByType<IFightController>();
            int count = all.Count;

            for (int i = 0; i < count; i++)
            {
                var obj = all[i];
                if (obj == null || obj.FightModel.CurrentHealth <= 0)
                    continue;

                DetectFor(obj);
            }
        }

        private void DetectFor(IFightController current)
        {
            var fightModel = current.FightModel;
            if (fightModel == null)
                return;

            Vector3 myPos = current.FightView.GetPosition();
            float radius = fightModel.DetectionRadius;
            float radiusSqr = radius * radius;
            ObjectController nearest = null;
            float nearestDist = radius;

            Vector2Int myCell = GetCell(myPos);

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    var cell = new Vector2Int(myCell.x + x, myCell.y + y);
                    if (!_grid.TryGetValue(cell, out var list))
                        continue;

                    for (int i = 0; i < list.Count; i++)
                    {
                        var other = list[i];
                        if (other == current || other.WarSide == fightModel.WarSide || other.CurrentHealth <= 0)
                            continue;

                        Vector3 delta = other.transform.position - myPos;
                        float sqrDist = delta.sqrMagnitude;
                        if (sqrDist < radiusSqr)
                        {
                            float dist = Mathf.Sqrt(sqrDist);
                            if (dist < nearestDist)
                            {
                                nearestDist = dist;
                                nearest = other;
                            }
                        }
                    }
                }
            }

            fightModel.AimObject = nearest;
        }
    }
}
