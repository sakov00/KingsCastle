using System;
using _Project.Scripts.Enums;
using _Project.Scripts.GameObjects.Abstract.Unit;
using MemoryPack;
using UnityEngine;
using ISavableModel = _Project.Scripts.Interfaces.ISavableModel;

namespace _Project.Scripts.GameObjects
{
    [Serializable]
    [MemoryPackable]
    public partial class PlayerModel : UnitModel
    {
        [field: Header("Resurrection")] 
        [MemoryPackIgnore][field:SerializeField] public int DurationTimeNoDamage { get; set; } = 2;
        [MemoryPackInclude][field:SerializeField] public int CurrentTimeNoDamage { get; set; }
        [MemoryPackInclude][field:SerializeField] public bool IsNoDamageable { get; set; }
        [MemoryPackIgnore][field:SerializeField] public int DurationTimeResurrection  { get; set; } = 3;
        [MemoryPackInclude][field:SerializeField] public int CurrentTimeResurrection { get; set; }
        [MemoryPackInclude][field:SerializeField] public bool IsKilled { get; set; }

        [field: Header("Abilities")]
        [MemoryPackIgnore][field: SerializeField] public int MaxValueUltimate { get; set; } = 100;

        [MemoryPackInclude][field: SerializeField] private int _currentValueUltimate;
        [MemoryPackIgnore] [field: SerializeField] public int ShootRewardValue { get; set; } = 10;
        [MemoryPackIgnore] [field: SerializeField] public int DurationUltimate { get; set; } = 5;
        [MemoryPackInclude] [field: SerializeField] public int CurrentTimeUltimate { get; set; }
        [MemoryPackInclude][field:SerializeField] public bool IsActiveUltimate { get; set; }
        [MemoryPackIgnore] [field: SerializeField] public float UltimateUpDamageModifier { get; set; } = 2f;
        
        // public override float CurrentHealth
        // {
        //     get => _currentHealth;
        //     set
        //     {
        //         if(IsNoDamageable) return;
        //         if (value < _currentHealth)
        //             SecondsWithoutDamage = 0;
        //         _currentHealth = Mathf.Clamp(value, 0, MaxHealth);
        //     }
        // }
        
        [MemoryPackInclude] public int CurrentValueUltimate
        {
            get => _currentValueUltimate;
            set => _currentValueUltimate = Mathf.Clamp(value, 0, MaxValueUltimate);
        }
    }
}