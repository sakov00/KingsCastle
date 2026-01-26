using _Project.Scripts.GameObjects;
using UnityEngine;
using PlayerModel = _Project.Scripts.GameObjects.PlayerModel;

namespace _Project.Scripts.ServicesGameplay
{
    public class PlayerMovementService
    {
        private readonly PlayerModel _playerModel;
        private readonly PlayerView _playerView;
        private readonly Transform _transform;
        private Vector3 _velocity;

        public PlayerMovementService(PlayerModel playerModel, PlayerView playerView, Transform transform)
        {
            _playerModel = playerModel;
            _playerView = playerView;
            _transform = transform;

            // playerView.Agent.updateRotation = false;
            // playerView.Agent.acceleration = 100f;
            // playerView.Agent.autoBraking = false;
        }

        public void MoveTo(Vector3 inputVector)
        {
            // if (!_playerView.Agent.isOnNavMesh) return;
            //
            // if (inputVector.sqrMagnitude < 0.01f)
            // {
            //     _playerView.Agent.isStopped = true;
            //     return;
            // }
            //
            // var direction = Vector3.ClampMagnitude(inputVector, 1f);
            // var destination = _transform.position + direction * 2f;
            //
            // _playerView.Agent.isStopped = false;
            // _playerView.Agent.speed = _playerModel.MoveSpeed;
            // _playerView.Agent.SetDestination(destination);
            //
            // if (_playerView.Agent.velocity.sqrMagnitude > 0.1f)
            // {
            //     var targetRotation = Quaternion.LookRotation(_playerView.Agent.velocity.normalized);
            //     _transform.rotation = Quaternion.Slerp(_transform.rotation, targetRotation,
            //         _playerModel.RotationSpeed * Time.deltaTime * 3f);
            // }
        }
        
        
    }
}