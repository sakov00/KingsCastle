using System;
using _Project.Scripts.Enums;
using MemoryPack;
using UnityEngine;
using BuildModel = _Project.Scripts.GameObjects.Abstract.Build.BuildModel;
using ISavableModel = _Project.Scripts.Interfaces.ISavableModel;

namespace _Project.Scripts.GameObjects
{
    [Serializable]
    [MemoryPackable]
    public partial class MoneyBuildModel : BuildModel
    {
        [Header("Money")] 
        [MemoryPackInclude][field:SerializeField] public int AddMoneyValue { get; set; } = 1;
    }
}