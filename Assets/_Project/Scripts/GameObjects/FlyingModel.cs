using System;
using _Project.Scripts.Enums;
using _Project.Scripts.GameObjects.Abstract.Unit;
using MemoryPack;
using ISavableModel = _Project.Scripts.Interfaces.ISavableModel;

namespace _Project.Scripts.GameObjects
{
    [Serializable]
    [MemoryPackable]
    public partial class FlyingModel : UnitModel
    {
    }
}