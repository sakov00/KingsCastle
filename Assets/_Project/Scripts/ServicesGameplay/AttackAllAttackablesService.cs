using _Project.Scripts.Interfaces;
using _Project.Scripts.Registries;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts.ServicesGameplay
{
    public class AttackAllAttackablesService : ITickable
    {
        [Inject] private LiveRegistry _liveRegistry;

        public void Tick()
        {
            var attackables = _liveRegistry.GetAllByType<IAttackable>();
            int count = attackables.Count;

            for (int i = 0; i < count; i++)
            {
                HandleAttack(attackables[i]);
            }
        }

        private void HandleAttack(IAttackable attackable)
        {
            if (attackable is not ISearchController searcher)
            {
                attackable.SetAttacking(false);
                return;
            }

            var target = searcher.CurrentAim;

            if (target == null)
            {
                attackable.SetAttacking(false);
                return;
            }

            float sqrDist = (target.transform.position - attackable.Position).sqrMagnitude;
            float attackRangeSqr = attackable.AttackRange * attackable.AttackRange;

            if (sqrDist <= attackRangeSqr)
            {
                attackable.SetAttacking(true);
            }
            else
            {
                attackable.SetAttacking(false);
            }
        }
    }
}