using _Project.Scripts.Enums;
using _Project.Scripts.Services;
using _Project.Scripts.UI.Windows.BaseWindow;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace _Project.Scripts.UI.Windows.PauseWindow
{
    public class PauseWindowView : BaseWindowView
    {
        [Inject] private SoundManager _soundManager;
        
        [Header("Presenter")]
        [SerializeField] private PauseWindowPresenter _presenter;

        [Header("Buttons")]
        [SerializeField] private Button _homeButton;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _continueButton;

        public override void Initialize()
        {
            base.Initialize();
            _presenter.HomeCommand.BindTo(_homeButton).AddTo(Disposables);
            _presenter.RestartCommand.BindTo(_restartButton).AddTo(Disposables);
            _presenter.ContinueCommand.BindTo(_continueButton).AddTo(Disposables);
            
            _homeButton.OnClickAsObservable()
                .Subscribe(_ => _soundManager.PlaySFX(SoundKey.ButtonClickSound))
                .AddTo(Disposables);

            _restartButton.OnClickAsObservable()
                .Subscribe(_ => _soundManager.PlaySFX(SoundKey.ButtonClickSound))
                .AddTo(Disposables);

            _continueButton.OnClickAsObservable()
                .Subscribe(_ => _soundManager.PlaySFX(SoundKey.ButtonClickSound))
                .AddTo(Disposables);
        }
        
        public override Tween Show()
        {
            var sequence = DOTween.Sequence();
            sequence.AppendCallback(() => Time.timeScale = 0);
            sequence.Append(base.Show());
            sequence.SetUpdate(true);
            return sequence;
        }

        public override Tween Hide()
        {
            var sequence = DOTween.Sequence();
            sequence.Append(base.Hide());
            sequence.AppendCallback(() => Time.timeScale = 1);
            sequence.SetUpdate(true);
            return sequence;
        }
        
        public override void ShowFast()
        {
            Time.timeScale = 0;
            base.ShowFast();
        }

        public override void HideFast()
        {
            Time.timeScale = 1;
            base.HideFast();
        }
    }
}