using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TweenControllers
{
    [RequireComponent(typeof(Button))]
    public class PlayOneClick : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private List<TweenAction> _tweens;
        [SerializeField] private bool _playSequentially = false;
        [SerializeField] private List<PlayOneClick> _clicks;

        private void OnValidate()
        {
            _button ??= GetComponent<Button>();
        }

        private void Awake()
        {
            _button.onClick.AddListener(OnClick);
        }

        public void Dispose()
        {
            _button.onClick.RemoveListener(OnClick);
        }

        private void OnDestroy()
        {
            Dispose();
        }

        public void OnClick()
        {
            _clicks.ForEach(x => x.Dispose());
            if (_tweens == null || _tweens.Count == 0)
                return;

            var validTweens = _tweens
                .Where(t => t != null)
                .Select(t => t.GetTween())
                .Where(t => t != null)
                .ToList();

            if (validTweens.Count == 0)
                return;

            if (_playSequentially)
            {
                var sequence = DOTween.Sequence();
                foreach (var tween in validTweens)
                    sequence.Append(tween);
                sequence.Play();
            }
            else
            {
                foreach (var tween in validTweens)
                    tween.Play();
            }
        }
    }
}