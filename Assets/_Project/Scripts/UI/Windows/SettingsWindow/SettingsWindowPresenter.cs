using _Project.Scripts.UI.Windows.BaseWindow;
using _Project.Scripts.UI.Windows.LoadingWindow;
using UniRx;
using UnityEngine;

namespace _Project.Scripts.UI.Windows.SettingsWindow
{
    public class SettingsWindowPresenter : BaseWindowPresenter
    {
        [SerializeField] private BaseWindowModel _model;
        [SerializeField] private SettingsWindowView _view;
        
        public override BaseWindowModel Model => _model;
        public override BaseWindowView View => _view;
        
        public ReactiveCommand SoundCommand { get; } = new();
        public ReactiveCommand MusicCommand { get; } = new();
        public ReactiveCommand VibroCommand { get; } = new();
        public ReactiveCommand PrivacyCommand { get; } = new();
        public ReactiveCommand TermsCommand { get; } = new();
        public ReactiveCommand BackCommand { get; } = new();

        public override void Initialize()
        {
            base.Initialize();
            
            SoundCommand.Subscribe(_ => SoundClick()).AddTo(Disposables);
            MusicCommand.Subscribe(_ => MusicClick()).AddTo(Disposables);
            VibroCommand.Subscribe(_ => VibroClick()).AddTo(Disposables);
            PrivacyCommand.Subscribe(_ => PrivacyClick()).AddTo(Disposables);
            TermsCommand.Subscribe(_ => TermsClick()).AddTo(Disposables);
            BackCommand.Subscribe(_ => BackClick()).AddTo(Disposables);
        }
        
        private void SoundClick() => WindowsManager.HideWindow<SettingsWindowPresenter>();
        private void MusicClick() => WindowsManager.HideWindow<SettingsWindowPresenter>();
        private void VibroClick() => WindowsManager.HideWindow<SettingsWindowPresenter>();
        private void PrivacyClick() => WindowsManager.HideWindow<SettingsWindowPresenter>();
        private void TermsClick() => WindowsManager.HideWindow<SettingsWindowPresenter>();
        private void BackClick() => WindowsManager.HideWindow<SettingsWindowPresenter>();
    }
}