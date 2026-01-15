using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.TweenFeature.TweenActions
{
    public class FadeTween : TweenAction
    {
        [Header("Targets")]
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Image _image;

        [Header("Settings")]
        [SerializeField] private bool _fadeIn = true;
        [SerializeField] private bool _useCurrentAsStart = false;
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private float _delay = 0f;
        [SerializeField] private Ease _ease = Ease.Linear;
        [SerializeField] private bool _controlRaycast = true;

        [Header("Fade Percent (0–100)")]
        [Range(0, 100)] [SerializeField] private float _fadeInPercent = 100f;
        [Range(0, 100)] [SerializeField] private float _fadeOutPercent = 0f;

        [Header("Loop")]
        [SerializeField] private LoopType _loopType = LoopType.Restart;
        [SerializeField] private int _loopsCount = 0;

        private void OnValidate()
        {
            if (_canvasGroup == null && _image == null)
            {
                Debug.LogWarning($"{nameof(FadeTween)}: не назначен ни один целевой компонент на {gameObject.name}");
            }
        }

        public override Tween GetTween()
        {
            if (_canvasGroup == null && _image == null)
            {
                Debug.LogWarning($"{nameof(FadeTween)}: не назначен ни один целевой компонент на {gameObject.name}");
                return DOTween.Sequence();
            }

            float start;
            if (_useCurrentAsStart)
            {
                start = _canvasGroup != null ? _canvasGroup.alpha : _image.color.a;
            }
            else
            {
                start = _fadeIn ? 0f : 1f;
                SetAlpha(start);
            }

            float end = _fadeIn
                ? _fadeInPercent / 100f
                : _fadeOutPercent / 100f;

            Tween tween = null;
            if(_canvasGroup != null)
                tween = _canvasGroup.DOFade(end, _duration);
            else
                tween =  _image.DOFade(end, _duration);

            tween
                .SetEase(_ease)
                .SetDelay(_delay);

            if (_loopsCount != 0)
            {
                tween.SetLoops(_loopsCount, _loopType);
            }

            if (_controlRaycast)
            {
                tween.OnStart(() =>
                {
                    if (!_fadeIn)
                        SetRaycast(false);
                });

                tween.OnComplete(() =>
                {
                    if (_fadeIn)
                        SetRaycast(true);
                });
            }

            return tween;
        }

        private void SetAlpha(float value)
        {
            if (_canvasGroup != null)
            {
                _canvasGroup.alpha = value;
                return;
            }

            if (_image == null) return;
            var color = _image.color;
            color.a = value;
            _image.color = color;
        }

        private void SetRaycast(bool state)
        {
            if (_canvasGroup != null)
                _canvasGroup.blocksRaycasts = state;
            else if (_image != null)
                _image.raycastTarget = state;
        }

        public void Play()
        {
            GetTween().Play();
        }
    }
}
