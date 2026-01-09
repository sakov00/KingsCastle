using _Project.Scripts.Enums;
using _Project.Scripts.Services;
using _Project.Scripts.UI.Windows.BaseWindow;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace _Project.Scripts.UI.Windows.WinWindow
{
    public class WinWindowView : BaseWindowView
    {
        [Inject] private SoundManager _soundManager;
        
        [Header("Presenter")]
        [SerializeField] private WinWindowPresenter _presenter;

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
    }
}