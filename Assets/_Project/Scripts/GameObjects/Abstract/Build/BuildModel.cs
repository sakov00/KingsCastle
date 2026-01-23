using System;
using _Project.Scripts.Enums;
using _Project.Scripts.GameObjects.Abstract.BaseObject;
using MemoryPack;
using UnityEngine;

namespace _Project.Scripts.GameObjects.Abstract.Build
{
    [Serializable]
    [MemoryPackable]
    [MemoryPackUnion(0, typeof(MoneyBuildModel))]
    [MemoryPackUnion(1, typeof(FriendsBuildModel))]
    [MemoryPackUnion(2, typeof(TowerDefenceModel))]
    public abstract partial class BuildModel : ObjectModel
    {
        [MemoryPackIgnore][field: SerializeField] public BuildType BuildType { get; protected set; }
        [MemoryPackIgnore][field: SerializeField] public int BuildPrice { get; set; }
    }
}