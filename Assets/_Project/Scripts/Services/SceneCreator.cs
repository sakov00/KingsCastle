using System;
using System.Collections.Generic;
using _Project.Scripts.Factories;
using _Project.Scripts.GameObjects.Abstract;
using _Project.Scripts.GameObjects.Abstract.Unit;
using _Project.Scripts.GameObjects.Additional.EnemyRoads;
using _Project.Scripts.GameObjects.Additional.LevelEnvironment.Terrain;
using _Project.Scripts.GameObjects.Concrete.BuildingZone;
using _Project.Scripts.GameObjects.Concrete.FriendsGroup;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Pools;
using Cysharp.Threading.Tasks;
using VContainer;

namespace _Project.Scripts.Services
{
    public class SceneCreator 
    {
        [Inject] private BuildPool _buildPool;
        [Inject] private UnitPool _unitPool;
        [Inject] private UnitFactory _unitFactory;
        [Inject] private OthersFactory _othersFactory;
        [Inject] private EnvironmentFactory _environmentFactory;
        
        private static readonly Dictionary<Type, int> TypePriority = new()
        {
            { typeof(EnemyRoadModel), 0 },
            { typeof(UnitModel), 1 },
            { typeof(BuildingZoneModel), 2 },
            { typeof(FriendsGroupModel), 3 },
            { typeof(BuildModel), 4 },
            { typeof(TerrainModel), 5 },
        };
        
        public async UniTask InstantiateObjects<T>(List<T> objects, bool isInitialize = true) where T : ISavableModel
        {
            SortSavableModels(objects);
            foreach (var model in objects)
            {
                ISavableController savableController = model switch
                {
                    BuildModel buildModel => 
                        _buildPool.Get(buildModel.BuildType, buildModel.SavePosition, buildModel.SaveRotation),
                    UnitModel unitModel => 
                        _unitPool.Get(unitModel.UnitType, unitModel.SavePosition, unitModel.SaveRotation),
                    FriendsGroupModel friendsGroupModel => 
                        _unitFactory.CreateFriendsGroup(friendsGroupModel.UnitType, friendsGroupModel.SavePosition, friendsGroupModel.SaveRotation),
                    TerrainModel terrainModel => 
                        _environmentFactory.CreateTerrain(terrainModel.SavePosition, terrainModel.SaveRotation),
                    EnemyRoadModel enemyRoadModel => 
                        _environmentFactory.CreateRoads(enemyRoadModel.SavePosition, enemyRoadModel.SaveRotation),
                    BuildingZoneModel buildingZoneModel => 
                        _othersFactory.CreateBuildingZone(buildingZoneModel.SavePosition, buildingZoneModel.SaveRotation),
                    _ => null
                };

                if(savableController == null)
                    continue;
                
                savableController.SetSavableModel(model);
                if(isInitialize) await savableController.InitializeAsync();
                
                await UniTask.Yield();
            }
        }
        
        public void SortSavableModels<T>(List<T> objects) where T : ISavableModel
        {
            objects.Sort((a, b) =>
            {
                int aPriority = GetPriority(a.GetType());
                int bPriority = GetPriority(b.GetType());
                return aPriority.CompareTo(bPriority);
            });
        }

        private int GetPriority(Type type)
        {
            if (TypePriority.TryGetValue(type, out var priority))
                return priority;

            foreach (var kvp in TypePriority)
            {
                if (kvp.Key.IsAssignableFrom(type))
                    return kvp.Value;
            }

            return int.MaxValue;
        }
    }
}