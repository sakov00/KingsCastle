using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;
using _Project.Scripts.AllAppData;
using _Project.Scripts.Enums;
using _Project.Scripts.GameObjects.Abstract.BaseObject;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Registries;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts.ServicesGameplay
{
    public class SpatialDetectionService : ITickable
    {
        [Inject] private AppData _appData;
        [Inject] private LiveRegistry _liveRegistry;

        private float _cellSize = 20f;

        private readonly Dictionary<Vector2Int, List<ObjectController>> _grid = new(128);

        // Кешируем позиции и ссылки для Job
        private NativeArray<float3> _positions;
        private NativeArray<WarSide> _warSides;
        private NativeArray<float> _health;
        private NativeArray<int> _indexes;

        public void Tick()
        {
            UpdateGrid();
            DetectAll();
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

            if (_positions.IsCreated) _positions.Dispose();
            if (_warSides.IsCreated) _warSides.Dispose();
            if (_health.IsCreated) _health.Dispose();
            if (_indexes.IsCreated) _indexes.Dispose();

            _positions = new NativeArray<float3>(count, Allocator.Persistent);
            _warSides = new NativeArray<WarSide>(count, Allocator.Persistent);
            _health = new NativeArray<float>(count, Allocator.Persistent);
            _indexes = new NativeArray<int>(count, Allocator.Persistent);

            for (int i = 0; i < count; i++)
            {
                var obj = all[i];
                Vector3 pos = obj.transform.position;
                Vector2Int cell = GetCell(pos);

                if (!_grid.TryGetValue(cell, out var list))
                {
                    list = new List<ObjectController>(8);
                    _grid[cell] = list;
                }
                list.Add(obj);

                _positions[i] = pos;
                _warSides[i] = obj.WarSide;
                _health[i] = obj.Model.CurrentHealth;
                _indexes[i] = i;
            }
        }

        public void DetectAll()
        {
            var allSearch = _liveRegistry.GetAllByType<ISearchController>();
            int count = allSearch.Count;

            var results = new NativeArray<int>(count, Allocator.TempJob);

            var job = new DetectNearestJob
            {
                positions = _positions,
                warSides = _warSides,
                health = _health,
                detectionPositions = new NativeArray<float3>(count, Allocator.TempJob),
                detectionRadii = new NativeArray<float>(count, Allocator.TempJob),
                results = results,
                indexes = _indexes
            };

            for (int i = 0; i < count; i++)
            {
                job.detectionPositions[i] = allSearch[i].Position;
                job.detectionRadii[i] = allSearch[i].DetectionRadius;
            }

            JobHandle handle = job.Schedule(count, 64);
            handle.Complete();

            // Назначаем результаты обратно объектам
            for (int i = 0; i < count; i++)
            {
                int nearestIndex = results[i];
                allSearch[i].CurrentAim = nearestIndex >= 0 ? _liveRegistry.GetAllReactive()[nearestIndex] : null;
            }

            job.detectionPositions.Dispose();
            job.detectionRadii.Dispose();
            results.Dispose();
        }

        [BurstCompile]
        struct DetectNearestJob : IJobParallelFor
        {
            [ReadOnly] public NativeArray<float3> positions;
            [ReadOnly] public NativeArray<WarSide> warSides;
            [ReadOnly] public NativeArray<float> health;

            [ReadOnly] public NativeArray<float3> detectionPositions;
            [ReadOnly] public NativeArray<float> detectionRadii;
            [ReadOnly] public NativeArray<int> indexes;

            public NativeArray<int> results;

            public void Execute(int index)
            {
                float3 myPos = detectionPositions[index];
                float radius = detectionRadii[index];
                float radiusSqr = radius * radius;

                int nearest = -1;
                float nearestDist = radius;

                for (int i = 0; i < positions.Length; i++)
                {
                    if (health[i] <= 0 || i == index)
                        continue;

                    if (warSides[i] == warSides[index])
                        continue;

                    float3 delta = positions[i] - myPos;
                    float sqrDist = math.lengthsq(delta);

                    if (sqrDist < radiusSqr)
                    {
                        float dist = math.sqrt(sqrDist);
                        if (dist < nearestDist)
                        {
                            nearestDist = dist;
                            nearest = indexes[i];
                        }
                    }
                }

                results[index] = nearest;
            }
        }
    }
}
