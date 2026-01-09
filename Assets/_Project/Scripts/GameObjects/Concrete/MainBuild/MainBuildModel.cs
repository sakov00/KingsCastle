using System;
using _Project.Scripts.Enums;
using _Project.Scripts.GameObjects.Abstract;
using _Project.Scripts.GameObjects.Abstract.BaseObject;
using _Project.Scripts.Interfaces;
using MemoryPack;
using UnityEngine;
using ISavableModel = _Project.Scripts.Interfaces.ISavableModel;

namespace _Project.Scripts.GameObjects.Concrete.MainBuild
{
    [Serializable]
    [MemoryPackable]
    public partial class MainBuildModel : BuildModel, IFightModel
    {
        [field: Header("Attack")] 
        [MemoryPackIgnore][field: SerializeField] public float AttackRange { get; set; } = 10f;
        [MemoryPackIgnore][field: SerializeField] public float DamageAmount { get; set; } = 10;
        [MemoryPackIgnore][field: SerializeField] public float AllAnimAttackTime { get; set; } = 1f;
        [MemoryPackIgnore][field: SerializeField] public float AnimAttackTime { get; set; } = 1f;
        [MemoryPackIgnore][field: SerializeField] public float DetectionRadius { get; set; } = 20f;
        [MemoryPackIgnore][field: SerializeField] public TypeAttack TypeAttack { get; set; } = TypeAttack.Distance;
        [MemoryPackIgnore][field: SerializeField] public ObjectController AimObject { get; set; }
        
        public override ISavableModel GetSaveData()
        {
            var model = new MainBuildModel();
            FillObjectModelData(model);
            FillBuildModelData(model);
            return model;
        }
    }
}