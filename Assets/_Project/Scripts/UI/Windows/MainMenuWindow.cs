using System.Collections.Generic;
using _Project.Scripts.AllAppData;
using _Project.Scripts.UI.WindowElements;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace _Project.Scripts.UI.Windows
{
    public class MainMenuWindow : BaseWindow
    {
        [Inject] private AppData _appData;
        
        [SerializeField] private Button _upgradeButton;
        [SerializeField] private Button _settingsButton;

        [Header("Levels")]
        [SerializeField] private LevelScrollRect _scrollRect;
        [SerializeField] private List<LevelPanel> _levelPanels = new();
        [SerializeField] private int _totalLevels = 100;

        protected override void Awake()
        {
            base.Awake();
            _upgradeButton.OnClickAsObservable().Subscribe(_ => OpenUpgradeWindow()).AddTo(Disposables);
            _settingsButton.OnClickAsObservable().Subscribe(_ => OpenSettingsWindow()).AddTo(Disposables);
        }

        public override void Initialize()
        {
            base.Initialize();
            InitializeLevelPanels(5);
        }
        
        private void OpenSettingsWindow() => WindowsManager.ShowWindow<SettingsWindow>();
        private void OpenUpgradeWindow() => WindowsManager.ShowWindow<UpgradeWindow>();

        private void InitializeLevelPanels(int currentLevel)
        {
            _scrollRect.Initialize(currentLevel, _levelPanels, _totalLevels);
        }
    }
}