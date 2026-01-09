using System;
using _Project.Scripts.Enums;
using _Project.Scripts.GameObjects.Abstract;
using _Project.Scripts.GameObjects.Concrete.FriendsGroup;
using _Project.Scripts.GameObjects.Concrete.MoneyBuild;
using MemoryPack;
using UnityEngine;
using ISavableModel = _Project.Scripts.Interfaces.ISavableModel;

namespace _Project.Scripts.GameObjects.Concrete.FriendsBuild
{
    [Serializable]
    [MemoryPackable]
    public partial class FriendsBuildModel : BuildModel
    {
        [Header("Units")] 
        [MemoryPackIgnore][field:SerializeField] public UnitType UnitType { get; set; }
        [MemoryPackIgnore][field:SerializeField] public int TimeCreateUnits { get; set; } = 5;
        [MemoryPackInclude][field:SerializeField] public int FriendsGroupId { get; set; }
        [MemoryPackInclude][field:SerializeField] public int NeedRestoreUnitsCount { get; set; }
        [MemoryPackInclude][field:SerializeField] public int CurrentSpawnTimer { get; set; } = -1;
        
        public override void LoadData(ISavableModel model)
        {
            base.LoadData(model);
            if (model is not FriendsBuildModel objectModel) return;
            FriendsGroupId = objectModel.FriendsGroupId;
            NeedRestoreUnitsCount = objectModel.NeedRestoreUnitsCount;
            CurrentSpawnTimer = objectModel.CurrentSpawnTimer;
        }
        
        public override ISavableModel GetSaveData()
        {
            var model = new FriendsBuildModel
            {
                FriendsGroupId = FriendsGroupId,
                NeedRestoreUnitsCount = NeedRestoreUnitsCount,
                CurrentSpawnTimer = CurrentSpawnTimer,
            };
            FillObjectModelData(model);
            FillBuildModelData(model);
            return model;
        }
    }
}