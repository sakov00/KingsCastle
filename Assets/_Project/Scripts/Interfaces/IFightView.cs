using System;
using _Project.Scripts.Enums;
using UnityEngine;

namespace _Project.Scripts.Interfaces
{
    public interface IFightView
    {
        public ProjectileType ProjectileType { get; set; }
        public Transform FirePoint { get; set; }
        public float ProjectileSpeed { get; set; }
        public event Action AttackHitEvent;
        public void SetWalking(bool isWalking);
        public void SetAttacking(bool isAttacking);
        public Vector3 GetPosition();
        public Vector3 GetScale();
    }
}