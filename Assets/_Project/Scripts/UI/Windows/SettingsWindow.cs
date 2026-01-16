using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Windows
{
    public class SettingsWindow : BaseWindow
    {
        [Header("UI Elements")]
        [SerializeField] private Button _soundButton;
        [SerializeField] private Button _musicButton;
        [SerializeField] private Button _vibroButton;
        [SerializeField] private Button _privacyButton;
        [SerializeField] private Button _termsButton;
        [SerializeField] private Button _backButton;

        protected override void Awake()
        {
            base.Awake();
            _soundButton.OnClickAsObservable().Subscribe(_ => SoundClick()).AddTo(Disposables);
            _musicButton.OnClickAsObservable().Subscribe(_ => MusicClick()).AddTo(Disposables);
            _vibroButton.OnClickAsObservable().Subscribe(_ => VibroClick()).AddTo(Disposables);
            _privacyButton.OnClickAsObservable().Subscribe(_ => PrivacyClick()).AddTo(Disposables);
            _termsButton.OnClickAsObservable().Subscribe(_ => TermsClick()).AddTo(Disposables);
            _backButton.OnClickAsObservable().Subscribe(_ => BackClick()).AddTo(Disposables);
        }
    
        private void SoundClick() => WindowsManager.HideWindow<SettingsWindow>();
        private void MusicClick() => WindowsManager.HideWindow<SettingsWindow>();
        private void VibroClick() => WindowsManager.HideWindow<SettingsWindow>();
        private void PrivacyClick() => WindowsManager.HideWindow<SettingsWindow>();
        private void TermsClick() => WindowsManager.HideWindow<SettingsWindow>();
        private void BackClick() => WindowsManager.HideWindow<SettingsWindow>();
    }
}
