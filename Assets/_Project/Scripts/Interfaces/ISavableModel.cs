

using _Project.Scripts.GameObjects.Abstract;
using _Project.Scripts.GameObjects.Abstract.BaseObject;
using _Project.Scripts.GameObjects.Abstract.Unit;
using _Project.Scripts.GameObjects.Additional.EnemyRoads;
using _Project.Scripts.GameObjects.Additional.LevelEnvironment.Environment;
using _Project.Scripts.GameObjects.Additional.LevelEnvironment.Terrain;
using _Project.Scripts.GameObjects.Concrete.ArcherEnemy;
using _Project.Scripts.GameObjects.Concrete.ArcherFriend;
using _Project.Scripts.GameObjects.Concrete.BuildingZone;
using _Project.Scripts.GameObjects.Concrete.FlyingEnemy;
using _Project.Scripts.GameObjects.Concrete.FriendsBuild;
using _Project.Scripts.GameObjects.Concrete.FriendsGroup;
using _Project.Scripts.GameObjects.Concrete.MainBuild;
using _Project.Scripts.GameObjects.Concrete.MoneyBuild;
using _Project.Scripts.GameObjects.Concrete.Player;
using _Project.Scripts.GameObjects.Concrete.TowerDefence;
using _Project.Scripts.GameObjects.Concrete.WarriorEnemy;
using _Project.Scripts.GameObjects.Concrete.WarriorFriend;
using MemoryPack;
using UnityEngine;
    
namespace _Project.Scripts.Interfaces
{
    [MemoryPackable]
    [MemoryPackUnion(0, typeof(BuildingZoneModel))]
    [MemoryPackUnion(1, typeof(EnemyRoadModel))]
    [MemoryPackUnion(2, typeof(EnvironmentModel))]
    [MemoryPackUnion(3, typeof(TerrainModel))]
    [MemoryPackUnion(4, typeof(ObjectModel))]
    [MemoryPackUnion(5, typeof(BuildModel))]
    [MemoryPackUnion(6, typeof(MainBuildModel))]
    [MemoryPackUnion(7, typeof(MoneyBuildModel))]
    [MemoryPackUnion(8, typeof(FriendsBuildModel))]
    [MemoryPackUnion(9, typeof(TowerDefenceModel))]
    [MemoryPackUnion(10, typeof(UnitModel))]
    [MemoryPackUnion(11, typeof(PlayerModel))]
    [MemoryPackUnion(12, typeof(WarriorEnemyModel))]
    [MemoryPackUnion(13, typeof(WarriorFriendModel))]
    [MemoryPackUnion(14, typeof(ArcherEnemyModel))]
    [MemoryPackUnion(15, typeof(ArcherFriendModel))]
    [MemoryPackUnion(16, typeof(FlyingEnemyModel))]
    [MemoryPackUnion(17, typeof(FriendsGroupModel))]
    public partial interface ISavableModel
    {
        public Vector3 SavePosition { get; set; }
        public Quaternion SaveRotation { get; set; }
        public void LoadData(ISavableModel savableModel);
        public ISavableModel GetSaveData();
    }
}