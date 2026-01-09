using _Project.Scripts.Enums;
using _Project.Scripts.GameObjects.Additional.EnemyRoads;
using _Project.Scripts.GameObjects.Additional.LevelEnvironment.Environment;
using _Project.Scripts.GameObjects.Additional.LevelEnvironment.Terrain;
using _Project.Scripts.SO;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts.Factories
{
    public class EnvironmentFactory
    {
        [Inject] private IObjectResolver _resolver;
        [Inject] private LevelsConfig _levelsConfig;
        
        public EnvironmentController CreateEnvironment(EnvironmentType environmentType, Vector3 position, Quaternion rotation = default)
        {
            EnvironmentController environmentController = null;
            switch (environmentType)
            {
                case EnvironmentType.Tree:
                    environmentController = null;
                    break;
            }
            return environmentController;
        }

        public TerrainController CreateTerrain(Vector3 position = default, Quaternion rotation = default)
        {
            return _resolver.Instantiate(_levelsConfig.terrainPrefab, position, rotation);
        }
        
        public EnemyRoadController CreateRoads(Vector3 position = default, Quaternion rotation = default)
        {
            return _resolver.Instantiate(_levelsConfig.roadPrefab, position, rotation);
        }
    }
}