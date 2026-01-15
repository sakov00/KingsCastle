using _Project.Scripts.UI.Windows.BaseWindow;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Windows.SettingsWindow
{
    public class SettingsWindowView : BaseWindowView
    {
        [Header("Presenter")]
        [SerializeField] private SettingsWindowPresenter _presenter;

        [Header("UI Elements")]
        [SerializeField] private Button _soundButton;
        [SerializeField] private Button _musicButton;
        [SerializeField] private Button _vibroButton;
        [SerializeField] private Button _privacyButton;
        [SerializeField] private Button _termsButton;
        [SerializeField] private Button _backButton;
        
        public override void Initialize()
        {
            base.Initialize();

            _presenter.SoundCommand.BindTo(_soundButton).AddTo(Disposables);
            _presenter.MusicCommand.BindTo(_musicButton).AddTo(Disposables);
            _presenter.VibroCommand.BindTo(_vibroButton).AddTo(Disposables);
            _presenter.PrivacyCommand.BindTo(_privacyButton).AddTo(Disposables);
            _presenter.TermsCommand.BindTo(_termsButton).AddTo(Disposables);
            _presenter.BackCommand.BindTo(_backButton).AddTo(Disposables);
        }
    }
}