using System;
using _Project.Scripts.GameObjects.Abstract.Unit;
using MemoryPack;
using ISavableModel = _Project.Scripts.Interfaces.ISavableModel;

namespace _Project.Scripts.GameObjects.Concrete.FlyingEnemy
{
    [Serializable]
    [MemoryPackable]
    public partial class FlyingEnemyModel : UnitModel
    {
        public override ISavableModel GetSaveData()
        {
            var model = new FlyingEnemyModel();
            FillObjectModelData(model);
            FillUnitModelData(model);
            return model;
        }
    }
}