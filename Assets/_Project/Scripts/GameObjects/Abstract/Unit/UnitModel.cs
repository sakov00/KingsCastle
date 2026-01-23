using System;
using System.Collections.Generic;
using _Project.Scripts.Enums;
using _Project.Scripts.GameObjects.Abstract.BaseObject;
using MemoryPack;
using UnityEngine;

namespace _Project.Scripts.GameObjects.Abstract.Unit
{
    [Serializable]
    [MemoryPackable]
    [MemoryPackUnion(0, typeof(PlayerModel))]
    [MemoryPackUnion(1, typeof(WarriorModel))]
    [MemoryPackUnion(2, typeof(ArcherModel))]
    [MemoryPackUnion(3, typeof(FlyingModel))]
    public abstract partial class UnitModel : ObjectModel
    {
        [MemoryPackIgnore][field: SerializeField] public UnitType UnitType { get; protected set; }
        
        [field: Header("Movement")] 
        [MemoryPackIgnore][field: SerializeField] public float MoveSpeed { get; set; } = 10f;
        [MemoryPackIgnore][field: SerializeField] public float RotationSpeed { get; set; } = 10f;

        [field: Header("Attack")] 
        [MemoryPackIgnore][field: SerializeField] public float AttackRange { get; set; } = 10f;
        [MemoryPackIgnore][field: SerializeField] public float DefaultDamageAmount { get; set; } = 10;
        [MemoryPackInclude][field: SerializeField] public float DamageAmount { get; set; } = 10;
        [MemoryPackIgnore][field: SerializeField] public float DetectionRadius { get; set; } = 20f;
        [MemoryPackIgnore][field: SerializeField] public ObjectController AimObject { get; set; }
        
        [field: Header("Way Info")] 
        [MemoryPackInclude][field: SerializeField] public int CurrentWaypointIndex { get; set; }
        [MemoryPackInclude][field: SerializeField] public List<Vector3> WayToAim { get; set; }
    }
}