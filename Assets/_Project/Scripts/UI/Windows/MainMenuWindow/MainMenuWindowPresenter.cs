using _Project.Scripts.AllAppData;
using _Project.Scripts.UI.WindowElements;
using _Project.Scripts.UI.Windows.BaseWindow;
using _Project.Scripts.UI.Windows.SettingsWindow;
using UniRx;
using UnityEngine;
using VContainer;

namespace _Project.Scripts.UI.Windows.MainMenuWindow
{
    public class MainMenuWindowPresenter : BaseWindowPresenter
    {
        [Inject] private AppData _appData;
        
        [SerializeField] private BaseWindowModel _model;
        [SerializeField] private MainMenuWindowView _view;
        
        public override BaseWindowModel Model => _model;
        public override BaseWindowView View => _view;
        
        public ReactiveCommand OpenSettingsWindowCommand { get; } = new();
        public ReactiveCommand OpenUpgradeWindowCommand { get; } = new();

        public override void Initialize()
        {
            base.Initialize();
            
            OpenSettingsWindowCommand.Subscribe(_ => OpenSettingsWindow()).AddTo(Disposables);
            OpenUpgradeWindowCommand.Subscribe(_ => OpenUpgradeWindow()).AddTo(Disposables);
            _view.InitializeLevelPanels(_appData.User.CurrentLevel);
        }
        
        private void OpenSettingsWindow() => WindowsManager.ShowWindow<SettingsWindowPresenter>();
        private void OpenUpgradeWindow() => WindowsManager.ShowWindow<UpgradeWindowPresenter>();
    }
}