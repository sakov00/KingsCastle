using _Project.Scripts.Factories;
using _Project.Scripts.GameObjects.Abstract.Unit;
using _Project.Scripts.Pools;
using VContainer;

namespace _Project.Scripts.GameObjects
{
    public class ArcherController : UnitController<ArcherModel, ArcherView>
    {
        [Inject] private ProjectilePool _projectilePool;
        public override void Initialize()
        {
            base.Initialize();
            
            Model.CurrentHealth = Model.MaxHealth;
            View.Initialize();
        }
        
        public override void Attack()
        {
            var projectile = _projectilePool.Get(View.ProjectileType, View.FirePoint.position, View.FirePoint.rotation);
            projectile.OwnerWarSide = Model.WarSide;
            projectile.Damage = Model.DamageAmount;
            projectile.PowerAttack = Model.PowerAttack;
            projectile.Speed = View.ProjectileSpeed;
            projectile.LaunchToPoint(CurrentAim.transform.position, CurrentAim);
        }
    }
}