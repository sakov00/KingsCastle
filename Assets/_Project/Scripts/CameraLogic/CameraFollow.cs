using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace _Project.Animation.CameraLogic
{
    [Serializable]
    public class CameraFollow
    {
        [SerializeField] private Vector3 _offset = new(0f, 20f, -12f);
        [SerializeField] private float _followSpeed = 5f;
        [SerializeField] private float _returnSpeed = 30f;
        [SerializeField] private float _limitSecondsReturn = 1f;
        
        private Transform _cameraTransform;
        private Transform _target;
        private Tween _moveTween;
        private bool _isFollow;

        public void Init(Transform cameraTransformParam, Transform targetParam)
        {
            _cameraTransform = cameraTransformParam;
            _target = targetParam;
            _cameraTransform.position = _target.position + _offset;
            _cameraTransform.LookAt(_target);
            _isFollow = true;
        }

        public async UniTask EnableFollowAnimation()
        {
            _moveTween?.Kill();
            if (_target == null) return; 
            var desiredPosition = _target.position + _offset;

            var distance = Vector3.Distance(_cameraTransform.position, desiredPosition);
            if (distance < 0.01f) return;

            var duration = Mathf.Min(distance / _returnSpeed, _limitSecondsReturn);

            _moveTween?.Kill();
            _moveTween = _cameraTransform.DOMove(desiredPosition, duration).SetEase(Ease.Linear);
            await _moveTween.Play();
            _isFollow = true;
        }
        
        public void DisableFollowAnimation()
        {
            _moveTween?.Kill();
            _isFollow = false;
        }

        public void UpdateCameraPosition()
        {
            if (_target == null) return; 
            if (!_isFollow) return; 
            
            Vector3 desiredPosition = _target.position + _offset;
            _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, desiredPosition, _followSpeed * Time.deltaTime);
            _cameraTransform.LookAt(_target);
        }
    }
}
