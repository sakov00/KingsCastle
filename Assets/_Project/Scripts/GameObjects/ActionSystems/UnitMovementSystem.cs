using System.Collections.Generic;
using _Project.Scripts.Extentions;
using _Project.Scripts.GameObjects.Abstract.Unit;
using UnityEngine;
using UnityEngine.AI;

namespace _Project.Scripts.GameObjects.ActionSystems
{
    public class UnitMovementSystem
    {
        private readonly Transform _transform;
        private readonly UnitView _unitView;
        private readonly UnitModel _unitModel;

        private Vector3 _lastDestination;
        private Vector3 _lastFinalWaypoint;
        private int _lastBestIndex = -1;

        private const float DestinationUpdateThresholdSqr = 0.25f;
        private const float FinalPointChangeThresholdSqr = 1f;

        public UnitMovementSystem(UnitModel unitModel, UnitView unitView, Transform transform)
        {
            _transform = transform;
            _unitView = unitView;
            _unitModel = unitModel;
        }

        public void MoveToAim()
        {
            if (!_unitView.Agent.isOnNavMesh)
                return;
            
            var thresholdSqr = 0.5f;

            if (_unitModel.AimObject != null && !_unitModel.AimObject.Equals(null))
            {
                var distance = PositionExtension.GetDistanceBetweenObjects(
                    _transform.position,
                    _transform.localScale,
                    _unitModel.AimObject.transform.position,
                    _unitModel.AimObject.transform.localScale,
                    out var nearestPointA,
                    out var nearestPointB);
                
                bool inAttackRange = distance < _unitModel.AttackRange;
                _unitView.SetWalking(!inAttackRange);
                
                if (!inAttackRange)
                    TrySetDestination(nearestPointB);

                return;
            }
            
            _unitView.SetWalking(true);

            if (_unitModel.WayToAim == null || _unitModel.WayToAim.Count == 0)
                return;

            var finalDestination = _unitModel.WayToAim[_unitModel.WayToAim.Count - 1];

            bool finalChanged = (_lastFinalWaypoint - finalDestination).sqrMagnitude > FinalPointChangeThresholdSqr;

            if (finalChanged || _lastBestIndex == -1)
            {
                _lastBestIndex = FindBestWaypointIndex(_transform.position, _unitModel.WayToAim);
                _lastFinalWaypoint = finalDestination;
            }

            if (_unitModel.CurrentWaypointIndex < _unitModel.WayToAim.Count - 1)
            {
                var currentTarget = _unitModel.WayToAim[_unitModel.CurrentWaypointIndex];
                if ((_transform.position - currentTarget).sqrMagnitude < thresholdSqr)
                    _unitModel.CurrentWaypointIndex++;
            }

            if (_lastBestIndex > _unitModel.CurrentWaypointIndex)
                _unitModel.CurrentWaypointIndex = _lastBestIndex;

            var nextPoint = _unitModel.WayToAim[_unitModel.CurrentWaypointIndex];
            TrySetDestination(nextPoint);
            _unitView.Agent.isStopped = false;
        }

        private void TrySetDestination(Vector3 newDestination)
        {
            if ((_lastDestination - newDestination).sqrMagnitude > DestinationUpdateThresholdSqr)
            {
                _unitView.Agent.SetDestination(newDestination);
                _lastDestination = newDestination;
            }
        }

        private int FindBestWaypointIndex(Vector3 currentPosition, List<Vector3> waypoints)
        {
            if (waypoints.Count == 0)
                return 0;

            return waypoints.Count - 1;
        }

        private float GetPathLength(NavMeshPath path)
        {
            float length = 0f;
            if (path.corners.Length < 2)
                return length;

            for (int i = 1; i < path.corners.Length; i++)
                length += Vector3.Distance(path.corners[i - 1], path.corners[i]);

            return length;
        }
    }
}
