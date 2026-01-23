using _Project.Scripts.Enums;
using _Project.Scripts.GameObjects.Abstract.BaseObject;

namespace _Project.Scripts.Interfaces
{
    public interface IFightModel
    {
        public WarSide WarSide { get; }
        public float DelayRegeneration { get; }
        public float RegenerateHealthInSecond { get; }
        public int SecondsWithoutDamage { get; set; }
        public float MaxHealth { get; }
        public float CurrentHealth { get; set; }
        
        public float AttackRange { get; set; }
        public float DamageAmount { get; set; }
        public float AllAnimAttackTime { get; set; }
        public float AnimAttackTime { get; set; }
        public float DetectionRadius { get; set; }
        public ObjectController AimObject { get; set; }
    }
}