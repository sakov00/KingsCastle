using System.Collections.Generic;
using _Project.Scripts.GameObjects.Abstract.BaseObject;
using _Project.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace _Project.Scripts.GameObjects.Abstract.Unit
{
    public class UnitView : ObjectView
    {
        private static readonly int IsWalking = Animator.StringToHash("IsWalking");
        private static readonly int IsAttack = Animator.StringToHash("IsAttack");
        private static readonly int Rotate = Animator.StringToHash("Rotate");

        [SerializeField] private Animator _animator;
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private List<Rigidbody> _allRigidbodies;
        
        private NavMeshPath _path;
        
        public bool IsMoving => _animator != null && _animator.GetBool(IsWalking);
        
        public override void Initialize()
        {
            base.Initialize();
            RagdollIsActive(false);
            _path = new NavMeshPath();
        }

        public void RagdollIsActive(bool isActive, Vector3? forceDirection = null, float forceAmount = 0f)
        {
            if (_animator == null) return;
            _animator.enabled = !isActive;
            _allRigidbodies.ForEach(rigidbody =>
            {
                rigidbody.isKinematic = !isActive;
                
                if (isActive && forceDirection.HasValue)
                {
                    rigidbody.AddForce(forceDirection.Value.normalized * forceAmount, ForceMode.Impulse);
                }
            });
        }

        public virtual void SetWalking(bool isWalking)
        {
            if (_animator == null) return;
            if (_animator.GetBool(IsWalking) == isWalking)
                return;
            
            _animator.SetBool(IsWalking, isWalking);
        }

        public virtual void SetAttacking(bool isAttacking)
        {
            if (_animator == null) return;
            if (_animator.GetBool(IsAttack) == isAttacking)
                return;
            
            _animator.SetBool(IsAttack, isAttacking);
        }
        
        public virtual void SetRotate()
        {
            if (_animator == null) return;
            _animator.SetTrigger(Rotate);
        }
        
        public void Select()
        {
            EnableOutline(true);
        }

        public void Deselect()
        {
            EnableOutline(false);
        }

        public void MoveTo(Transform target)
        {
            if (!_agent.enabled || IsMoving)
                return;

            var point = GetAttackPoint(target.position);

            _agent.isStopped = false;
            _agent.SetDestination(point);

            SetAttacking(false);
            SetWalking(true);
        }
        
        public void Stop()
        {
            if (!_agent.enabled)
                return;

            _agent.isStopped = true;
            _agent.ResetPath();

            SetWalking(false);
        }

        public Vector3 GetAttackPoint(Vector3 targetPosition)
        {
            if (!_agent.enabled)
                return targetPosition;
            
            if (!NavMesh.Raycast(_transform.position, targetPosition, out NavMeshHit hit, NavMesh.AllAreas))
            {
                hit.position = targetPosition;
            }

            if (!NavMesh.CalculatePath(_transform.position, hit.position, NavMesh.AllAreas, _path))
                return targetPosition;

            if (_path.corners.Length == 0)
                return targetPosition;

            Vector3 lastCorner = _path.corners[_path.corners.Length - 1];

            Vector2 offset2D = Random.insideUnitCircle * 0.5f;
            Vector3 offset = new Vector3(offset2D.x, 0, offset2D.y);

            return lastCorner + offset;
        }
    }
}
