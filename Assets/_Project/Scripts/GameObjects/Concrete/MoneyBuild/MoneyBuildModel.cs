using System;
using _Project.Scripts.GameObjects.Abstract;
using _Project.Scripts.GameObjects.Concrete.Player;
using MemoryPack;
using UnityEngine;
using ISavableModel = _Project.Scripts.Interfaces.ISavableModel;

namespace _Project.Scripts.GameObjects.Concrete.MoneyBuild
{
    [Serializable]
    [MemoryPackable]
    public partial class MoneyBuildModel : BuildModel
    {
        [Header("Money")] 
        [MemoryPackInclude][field:SerializeField] public int AddMoneyValue { get; set; } = 1;
        
        public override void LoadData(ISavableModel model)
        {
            base.LoadData(model);
            if (model is MoneyBuildModel objectModel)
            {
                AddMoneyValue = objectModel.AddMoneyValue;
            }
        }
        public override ISavableModel GetSaveData()
        {
            var model = new MoneyBuildModel
            {
                AddMoneyValue = AddMoneyValue,
            };
            FillObjectModelData(model);
            FillBuildModelData(model);
            return model;
        }
    }
}