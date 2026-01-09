using System;
using _Project.Scripts.Enums;
using _Project.Scripts.GameObjects.Concrete.ArcherEnemy;
using _Project.Scripts.GameObjects.Concrete.ArcherFriend;
using _Project.Scripts.GameObjects.Concrete.FlyingEnemy;
using _Project.Scripts.GameObjects.Concrete.FriendsBuild;
using _Project.Scripts.GameObjects.Concrete.MoneyBuild;
using _Project.Scripts.GameObjects.Concrete.Player;
using _Project.Scripts.GameObjects.Concrete.TowerDefence;
using _Project.Scripts.GameObjects.Concrete.WarriorEnemy;
using _Project.Scripts.GameObjects.Concrete.WarriorFriend;
using MemoryPack;
using UnityEngine;
using ISavableModel = _Project.Scripts.Interfaces.ISavableModel;
using UnitModel = _Project.Scripts.GameObjects.Abstract.Unit.UnitModel;

namespace _Project.Scripts.GameObjects.Abstract.BaseObject
{
    [Serializable]
    [MemoryPackable]
    [MemoryPackUnion(0, typeof(BuildModel))]
    [MemoryPackUnion(1, typeof(MoneyBuildModel))]
    [MemoryPackUnion(2, typeof(FriendsBuildModel))]
    [MemoryPackUnion(3, typeof(TowerDefenceModel))]
    [MemoryPackUnion(4, typeof(UnitModel))]
    [MemoryPackUnion(5, typeof(PlayerModel))]
    [MemoryPackUnion(6, typeof(WarriorEnemyModel))]
    [MemoryPackUnion(7, typeof(WarriorFriendModel))]
    [MemoryPackUnion(8, typeof(ArcherEnemyModel))]
    [MemoryPackUnion(9, typeof(ArcherFriendModel))]
    [MemoryPackUnion(10, typeof(FlyingEnemyModel))]
    public abstract partial class ObjectModel : ISavableModel
    {
        [MemoryPackInclude][field:SerializeField] public int Id { get; set; }
        
        [field: Header("Health")] 
        [MemoryPackIgnore][field:SerializeField] public WarSide WarSide { get; set; }
        [MemoryPackIgnore][field:SerializeField] public float DelayRegeneration { get; set; } = 3f;
        [MemoryPackIgnore][field:SerializeField] public float RegenerateHealthInSecond { get; set; } = 5f;
        [MemoryPackInclude][field:SerializeField] public int SecondsWithoutDamage { get; set; }
        [MemoryPackIgnore] [field: SerializeField] public float MaxHealth { get; set; } = 100f;
        [MemoryPackInclude][SerializeField] protected float _currentHealth;
        [MemoryPackInclude] public Vector3 SavePosition { get; set; }
        [MemoryPackInclude] public Quaternion SaveRotation { get; set; }
        [MemoryPackIgnore] public virtual float CurrentHealth
        {
            get => _currentHealth;
            set
            {
                if (value < _currentHealth)
                    SecondsWithoutDamage = 0;
                _currentHealth = Mathf.Clamp(value, 0, MaxHealth);
            }
        }

        public virtual void LoadData(ISavableModel model)
        {
            if (model is not ObjectModel objectModel) return;
            Id = objectModel.Id;
            SecondsWithoutDamage = objectModel.SecondsWithoutDamage;
            _currentHealth = objectModel._currentHealth;
        }

        public abstract ISavableModel GetSaveData();
        protected void FillObjectModelData(ObjectModel model)
        {
            model.Id = Id;
            model.SecondsWithoutDamage = SecondsWithoutDamage;
            model._currentHealth = _currentHealth;
            model.SavePosition = SavePosition;
            model.SaveRotation = SaveRotation;
        }
    }
}