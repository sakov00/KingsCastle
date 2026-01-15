using System;
using _Project.Scripts.UI.Windows.BaseWindow;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI.Windows.LoadingWindow
{
    public class LoadingWindowView : BaseWindowView
    {
        [Header("Presenter")]
        [SerializeField] private LoadingWindowPresenter _presenter;

        [Header("UI Elements")]
        [SerializeField] private TMP_Text _loadingText;
        
        [Header("Load Tween Settings")]
        [SerializeField] private string _baseText = "Loading";
        [SerializeField] private float _interval = 0.3f;
        private int _dotsCount = 0;
        
        private Tween _loadTween;

        private void StartLoadingTween()
        {
            _loadTween = DOTween.Sequence()
                .AppendCallback(UpdateText)
                .AppendInterval(_interval)
                .SetLoops(-1);
        }
        
        private void StopLoadingTween()
        {
            _loadTween.Kill();
        }
        
        private void UpdateText()
        {
            _dotsCount = (_dotsCount + 1) % 4;
            _loadingText.text = _baseText + new string('.', _dotsCount);
        }

        public override Tween Show()
        {
            if(_isShowed == true) return _tweenShow;
 
            StartLoadingTween();
            base.Show();
            return _tweenShow;
        }
        
        public override Tween Hide()
        {
            if(_isShowed == false) return _tweenHide;
            
            StopLoadingTween();
            base.Hide();
            return _tweenHide;
        }
        
        public override void ShowFast()
        {
            base.ShowFast();
            StartLoadingTween();
        }

        public override void HideFast()
        {
            base.HideFast();
            StopLoadingTween();
        }
    }
}