using _Project.Scripts._VContainer;
using _Project.Scripts.Enums;
using _Project.Scripts.Extentions;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Pools;
using UnityEngine;
using VContainer;

namespace _Project.Scripts.ServicesGameplay
{
    public class AttackService
    {
        [Inject] private ProjectilePool _projectilePool;

        private readonly IFightController _controller;
        private readonly Transform _transform;
        private float _lastAttackTime = -Mathf.Infinity;

        private IFightModel Model => _controller.FightModel;
        private IFightView View => _controller.FightView;

        public AttackService(IFightController controller, Transform transform)
        {
            _controller = controller;
            _transform = transform;

            InjectManager.Inject(this);
            SubscribeToAttackEvent();
        }

        private void SubscribeToAttackEvent()
        {
            // switch (Model.TypeAttack)
            // {
            //     case TypeAttack.Melee:
            //         View.AttackHitEvent += MeleeAttack;
            //         break;
            //     case TypeAttack.Distance:
            //         View.AttackHitEvent += DistanceAttack;
            //         break;
            // }
        }

        public void Attack()
        {
            if (Model.AimObject == null || Model.AimObject.Equals(null))
                return;

            if (Time.time - _lastAttackTime < Model.AllAnimAttackTime)
                return;

            if (!IsTargetInRange())
                return;
            
            _lastAttackTime = Time.time;
            View.SetAttacking(true);
        }

        private void MeleeAttack()
        {
            // if (!IsTargetValidAndInRange())
            //     return;
            //
            // var target = Model.AimObject;
            // target.CurrentHealth -= Model.DamageAmount;
            //
            // if (target.CurrentHealth <= 0)
            //     target.Killed();
        }

        private void DistanceAttack()
        {
            if (View.FirePoint == null || Model.AimObject == null)
                return;

            var projectile = _projectilePool.Get(View.ProjectileType, View.FirePoint.position);
            projectile.Owner = _controller;
            projectile.OwnerWarSide = Model.WarSide;
            projectile.Damage = Model.DamageAmount;

            // var targetPosition = Model.AimObject.transform.position +
            //                      Vector3.up * (Model.AimObject.HeightObject / 2f);

            // projectile.LaunchToPoint(targetPosition, View.ProjectileSpeed);
        }

        private bool IsTargetValidAndInRange()
        {
            if (Model.AimObject == null)
                return false;

            return IsTargetInRange();
        }

        private bool IsTargetInRange()
        {
            if (Model.AimObject == null)
                return false;
            
            var distance = PositionExtension.GetDistanceBetweenObjects(_transform.position, _transform.localScale,
                Model.AimObject.transform.position, Model.AimObject.transform.localScale);
            return distance <= Model.AttackRange;
        }

        public void Dispose()
        {
            // switch (Model.TypeAttack)
            // {
            //     case TypeAttack.Melee:
            //         View.AttackHitEvent -= MeleeAttack;
            //         break;
            //     case TypeAttack.Distance:
            //         View.AttackHitEvent -= DistanceAttack;
            //         break;
            // }
        }
    }
}
