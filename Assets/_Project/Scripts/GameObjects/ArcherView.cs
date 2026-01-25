using _Project.Scripts.Enums;
using _Project.Scripts.GameObjects.Abstract.Unit;
using UnityEngine;

namespace _Project.Scripts.GameObjects
{
    public class ArcherView : UnitView
    {
        [field: SerializeField] public ProjectileType ProjectileType { get; set; }
        [field: SerializeField] public Transform FirePoint { get; set; }
        [field: SerializeField] public float ProjectileSpeed { get; set; } = 40f;
        
        public override void SetWalking(bool isWalking)
        {
        }

        public override void SetAttacking(bool isAttacking)
        {
            OnAttackHit();
        }
    }
}