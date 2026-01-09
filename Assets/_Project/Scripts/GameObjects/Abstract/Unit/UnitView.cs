using System;
using _Project.Scripts.Enums;
using _Project.Scripts.GameObjects.Abstract.BaseObject;
using _Project.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace _Project.Scripts.GameObjects.Abstract.Unit
{
    public class UnitView : ObjectView, IFightView
    {
        private static readonly int IsWalking = Animator.StringToHash("IsWalking");
        private static readonly int IsAttack = Animator.StringToHash("IsAttack");

        [SerializeField] private Animator _animator;
        [field: SerializeField] public NavMeshAgent Agent { get; private set; }
        [field: SerializeField] public ProjectileType ProjectileType { get; set; }
        [field: SerializeField] public Transform FirePoint { get; set; }
        [field: SerializeField] public float ProjectileSpeed { get; set; } = 40f;

        public event Action AttackHitEvent;

        private bool _isAttacking;
        private bool _isWalking;
        
        public override void Initialize()
        {
            base.Initialize();
            _isWalking = true;
            _isAttacking = false;
            UpdateAnimatorState();
        }

        protected virtual void Update()
        {
            if (_animator == null) return;

            var state = _animator.GetCurrentAnimatorStateInfo(0);
            Agent.isStopped = !state.IsName("Walking");
        }

        public virtual void SetWalking(bool isWalking)
        {
            if (_isWalking == isWalking) return;
            _isWalking = isWalking;

            if (isWalking)
                _isAttacking = false;
            UpdateAnimatorState();
        }

        public virtual void SetAttacking(bool isAttacking)
        {
            if (_isAttacking == isAttacking) return;
            _isAttacking = isAttacking;

            if (isAttacking)
                _isWalking = false;
            UpdateAnimatorState();
        }

        protected virtual void UpdateAnimatorState()
        {
            if (_animator == null) return;

            _animator.SetBool(IsWalking, _isWalking && !_isAttacking);
            _animator.SetBool(IsAttack, _isAttacking);
        }

        public void OnAttackHit()
        {
            AttackHitEvent?.Invoke();
        }

        public Vector3 GetPosition() => transform.position;
        public Vector3 GetScale() => transform.localScale;
    }
}
