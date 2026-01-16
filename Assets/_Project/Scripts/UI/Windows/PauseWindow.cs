using _Project.Scripts.AllAppData;
using _Project.Scripts.Enums;
using _Project.Scripts.Services;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace _Project.Scripts.UI.Windows
{
    public class PauseWindow : BaseWindow
    {
        [Inject] private AppData _appData;
        [Inject] private GameManager _gameManager;
        [Inject] private SoundManager _soundManager;
        
        [Header("Buttons")]
        [SerializeField] private Button _homeButton;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _continueButton;

        public override void Initialize()
        {
            base.Initialize();
            _homeButton.OnClickAsObservable().Subscribe(_ =>
            {
                _soundManager.PlaySFX(SoundKey.ButtonClickSound);
                HomeOnClick();
            }).AddTo(Disposables);
            _restartButton.OnClickAsObservable().Subscribe(_ =>
            {
                _soundManager.PlaySFX(SoundKey.ButtonClickSound);
                RestartOnClick().Forget();
            }).AddTo(Disposables);
            _continueButton.OnClickAsObservable().Subscribe(_ =>
            {
                _soundManager.PlaySFX(SoundKey.ButtonClickSound);
                ContinueOnClick();
            }).AddTo(Disposables);
        }
        
        private void HomeOnClick()
        {
            WindowsManager.ShowWindow<MainMenuWindow>();
        }
        
        private async UniTaskVoid RestartOnClick()
        {
            await WindowsManager.HideWindow<PauseWindow>();
            await _gameManager.RestartLevel();
        }
        
        private void ContinueOnClick()
        {
            WindowsManager.HideWindow<PauseWindow>();
        }
    }
}