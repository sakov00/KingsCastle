using DG.Tweening;
using UnityEngine;

namespace _Project.Scripts.UI.TweenFeature.TweenActions
{
    public class RotationTween : TweenAction
    {
        [SerializeField] private Transform _target;

        [SerializeField] private Vector3 _rotationDelta = Vector3.zero;

        [Header("Tween Settings")]
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private float _delay = 0f;
        [SerializeField] private Ease _ease = Ease.Linear;
        [SerializeField] private bool _playOnAwake;
        [SerializeField] private bool _loop;
        [SerializeField] private RotateMode _rotateMode = RotateMode.Fast;

        private Tween _tween;

        private void Awake()
        {
            if (_playOnAwake)
                GetTween()?.Play();
        }

        public override Tween GetTween()
        {
            if (_target == null)
            {
                Debug.LogWarning($"{nameof(RotationTween)}: Target не найден на {gameObject.name}");
                return null;
            }
            
            Vector3 newRotation = _target.localEulerAngles + _rotationDelta;
            _tween = _target
                .DOLocalRotate(newRotation, _duration, _rotateMode);

            if (_loop)
                _tween.SetLoops(-1, LoopType.Restart);

            return _tween;
        }

        public void Play()
        {
            if(_tween == null || !_tween.active)
                GetTween()?.Play();
        }
    }
}
