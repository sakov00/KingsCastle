using System;
using _Project.Scripts.Enums;
using MemoryPack;
using UnityEngine;
using ISavableModel = _Project.Scripts.Interfaces.ISavableModel;
using UnitModel = _Project.Scripts.GameObjects.Abstract.Unit.UnitModel;

namespace _Project.Scripts.GameObjects.Abstract.BaseObject
{
    [Serializable]
    [MemoryPackable]
    [MemoryPackUnion(0, typeof(Build.BuildModel))]
    [MemoryPackUnion(1, typeof(MoneyBuildModel))]
    [MemoryPackUnion(2, typeof(FriendsBuildModel))]
    [MemoryPackUnion(3, typeof(TowerDefenceModel))]
    [MemoryPackUnion(4, typeof(UnitModel))]
    [MemoryPackUnion(5, typeof(PlayerModel))]
    [MemoryPackUnion(6, typeof(WarriorModel))]
    [MemoryPackUnion(7, typeof(ArcherModel))]
    [MemoryPackUnion(8, typeof(FlyingModel))]
    public abstract partial class ObjectModel : ISavableModel
    {
        [MemoryPackInclude][field:SerializeField] public WarSide WarSide { get; protected set; }
        
        [field: Header("Object Data")] 
        [MemoryPackIgnore][field:SerializeField] public float DelayRegeneration { get; set; } = 3f;
        [MemoryPackIgnore][field:SerializeField] public float RegenerateHealthInSecond { get; set; } = 5f;
        [MemoryPackInclude][field:SerializeField] public int SecondsWithoutDamage { get; set; }
        [MemoryPackInclude][field: SerializeField] public float MaxHealth { get; set; } = 100f;
        [MemoryPackInclude][field: SerializeField] public float CurrentHealth { get; set; } = 100f;
        [MemoryPackInclude] public Vector3 SavePosition { get; set; }
        [MemoryPackInclude] public Quaternion SaveRotation { get; set; }
    }
}