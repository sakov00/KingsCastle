using _Project.Scripts.GameObjects.Concrete.BuildingZone;
using _Project.Scripts.SO;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts.Factories
{
    public class OthersFactory
    {
        [Inject] private IObjectResolver _resolver;
        [Inject] private OthersPrefabConfig _othersPrefabConfig;
        
        public BuildingZone CreateBuildingZone(Vector3 position = default, Quaternion rotation = default)
        {
            return _resolver.Instantiate(_othersPrefabConfig.buildingZonePrefab, position, rotation);
        }
    }
}