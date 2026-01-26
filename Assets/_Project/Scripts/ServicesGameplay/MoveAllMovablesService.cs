using _Project.Scripts.GameObjects.Abstract.Unit;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Registries;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts.ServicesGameplay
{
    public class MoveAllMovablesService : ITickable
    {
        [Inject] private LiveRegistry _liveRegistry;

        public void Tick()
        {
            var movables = _liveRegistry.GetAllByType<IMovable>();
            int count = movables.Count;

            for (int i = 0; i < count; i++)
            {
                MoveSingle(movables[i]);
            }
        }

        private void MoveSingle(IMovable movable)
        {
            if (movable is not ISearchController searcher)
            {
                if (movable.IsMoving)
                    movable.Stop();
                return;
            }

            var target = searcher.CurrentAim;

            if (searcher.CurrentAim == null)
            {
                if (movable.IsMoving)
                    movable.Stop();
                return;
            }

            float sqrDist = (target.transform.position - movable.Position).sqrMagnitude;
            float stopDistSqr = movable.StopDistance * movable.StopDistance;

            if (sqrDist > stopDistSqr)
            {
                movable.MoveTo(searcher.CurrentAim.transform.position);
            }
            else
            {
                if (movable.IsMoving)
                    movable.Stop();
                
                RotateTowards(movable, target.transform.position);
            }
        }
        
        private void RotateTowards(IMovable movable, Vector3 targetPos)
        {
            var unitTransform = (movable as UnitController)?.transform;
            if (unitTransform == null) return;

            Vector3 direction = (targetPos - unitTransform.position).normalized;
            direction.y = 0; // чтобы не наклоняться вверх/вниз

            if (direction.sqrMagnitude > 0.001f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                unitTransform.rotation = Quaternion.Slerp(
                    unitTransform.rotation, 
                    lookRotation, 
                    Time.deltaTime * 10f // скорость поворота
                );
            }
        }
    }
}