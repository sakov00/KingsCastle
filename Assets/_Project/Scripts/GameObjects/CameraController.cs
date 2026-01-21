using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace _Project.Scripts.GameObjects
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        [field:SerializeField] public Camera CurrentCamera { get; set; }

        private void OnValidate()
        {
            CurrentCamera ??= new Camera();
        }
        
        [SerializeField] private Vector3 _offset = new(0f, 20f, -12f);
        [SerializeField] private float _followSpeed = 5f;
        [SerializeField] private float _returnSpeed = 30f;
        [SerializeField] private float _limitSecondsReturn = 1f;
        
        private Transform _target;
        private Tween _moveTween;
        private bool _isFollow;

        public void Initialize(Transform targetParam)
        {
            _target = targetParam;
            CurrentCamera.transform.position = _target.position + _offset;
            CurrentCamera.transform.LookAt(_target);
            _isFollow = true;
        }

        public async UniTask EnableFollowAnimation()
        {
            _moveTween?.Kill();
            if (_target == null) return; 
            var desiredPosition = _target.position + _offset;

            var distance = Vector3.Distance(CurrentCamera.transform.position, desiredPosition);
            if (distance < 0.01f) return;

            var duration = Mathf.Min(distance / _returnSpeed, _limitSecondsReturn);

            _moveTween?.Kill();
            _moveTween = CurrentCamera.transform.DOMove(desiredPosition, duration).SetEase(Ease.Linear);
            await _moveTween.Play();
            _isFollow = true;
        }
        
        public void DisableFollowAnimation()
        {
            _moveTween?.Kill();
            _isFollow = false;
        }

        private void LateUpdate()
        {
            if (_target == null) return; 
            if (!_isFollow) return; 
            
            Vector3 desiredPosition = _target.position + _offset;
            CurrentCamera.transform.position = Vector3.Lerp(CurrentCamera.transform.position, desiredPosition, _followSpeed * Time.deltaTime);
            CurrentCamera.transform.LookAt(_target);
        }
    }
}