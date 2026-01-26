using _Project.Scripts._VContainer;
using _Project.Scripts.Enums;
using _Project.Scripts.GameObjects.Abstract.BaseObject;
using _Project.Scripts.Pools;
using UnityEngine;
using VContainer;

namespace _Project.Scripts.GameObjects.Additional.Projectiles
{
    public abstract class Projectile : MonoBehaviour
    {
        [Inject] private ProjectilePool _projectilePool;

        [field: SerializeField] public WarSide OwnerWarSide { get; set; }
        [field: SerializeField] public ProjectileType ProjectileType { get; set; }
        [field: SerializeField] public float Damage { get; set; }
        [field: SerializeField] public float PowerAttack { get; set; }
        [field: SerializeField] public float Speed { get; set; } = 10f;

        protected Vector3 _targetPosition;
        protected ObjectController _target;
        protected bool _isLaunched;

        private void Start()
        {
            InjectManager.Inject(this);
        }

        private void OnEnable()
        {
            _isLaunched = false;
            CancelInvoke(nameof(ReturnToPool));
            Invoke(nameof(ReturnToPool), 5f); // авто-возврат через 5 секунд
        }

        private void Update()
        {
            if (!_isLaunched || _target == null) return;

            float step = Speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, step);

            // Если достигли цели — наносим урон
            if (Vector3.Distance(transform.position, _targetPosition) < 0.1f)
            {
                OnHit();
            }
        }

        protected virtual void OnHit()
        {
            if (_target != null && _target.WarSide != OwnerWarSide)
            {
                _target.TakeDamage(Damage, transform.forward, PowerAttack);
            }

            ReturnToPool();
        }

        /// <summary>
        /// Запуск снаряда к цели
        /// </summary>
        public virtual void LaunchToPoint(Vector3 targetPosition, ObjectController target)
        {
            _targetPosition = targetPosition;
            _target = target;
            _isLaunched = true;

            // Поворот снаряда в сторону цели
            Vector3 dir = (_targetPosition - transform.position).normalized;
            if (dir != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(dir);
        }

        protected void ReturnToPool()
        {
            CancelInvoke(nameof(ReturnToPool));
            _isLaunched = false;
            _projectilePool.Return(this);
        }
    }
}
