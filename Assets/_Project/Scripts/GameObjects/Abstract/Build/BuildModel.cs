using System;
using _Project.Scripts.Enums;
using _Project.Scripts.GameObjects.Abstract.BaseObject;
using _Project.Scripts.GameObjects.Concrete.FriendsBuild;
using _Project.Scripts.GameObjects.Concrete.MoneyBuild;
using _Project.Scripts.GameObjects.Concrete.TowerDefence;
using MemoryPack;
using UnityEngine;
using ISavableModel = _Project.Scripts.Interfaces.ISavableModel;

namespace _Project.Scripts.GameObjects.Abstract
{
    [Serializable]
    [MemoryPackable]
    [MemoryPackUnion(0, typeof(MoneyBuildModel))]
    [MemoryPackUnion(1, typeof(FriendsBuildModel))]
    [MemoryPackUnion(2, typeof(TowerDefenceModel))]
    public abstract partial class BuildModel : ObjectModel
    {
        [field: Header("Build Data")]
        [MemoryPackInclude][field: SerializeField] public BuildType BuildType { get; set; }
        [MemoryPackIgnore][field: SerializeField] public int BuildPrice { get; set; }
        
        public override void LoadData(ISavableModel model)
        {
            base.LoadData(model);
            if (model is not BuildModel objectModel) return;
            BuildType = objectModel.BuildType;
        }
        
        protected void FillBuildModelData(BuildModel model)
        {
            model.BuildType = BuildType;
        }
    }
}