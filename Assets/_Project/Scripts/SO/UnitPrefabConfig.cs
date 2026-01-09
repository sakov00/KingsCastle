using System.Collections.Generic;
using _Project.Animation.CameraLogic;
using _Project.Scripts.GameObjects.Abstract.Unit;
using _Project.Scripts.GameObjects.Concrete.FriendsGroup;
using UnityEngine;

namespace _Project.Scripts.SO
{
    [CreateAssetMenu(fileName = "UnitPrefabConfig", menuName = "SO/Unit Prefab Config")]
    public class UnitPrefabConfig : ScriptableObject
    {
        [Header("Unit Prefabs")] 
        public List<UnitController> allUnitPrefabs = new();
        public List<FriendsGroupController> allGroupController = new();
        public CameraFollow cameraPrefab;
    }
}