using System.Collections.Generic;
using _Project.Scripts.Enums;
using _Project.Scripts.UI.WindowElements;
using _Project.Scripts.UI.Windows.BaseWindow;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Windows.MainMenuWindow
{
    public class MainMenuWindowView : BaseWindowView
    {
        [Header("Presenter")]
        [SerializeField] private MainMenuWindowPresenter _presenter;

        [SerializeField] private Button _upgradeButton;
        [SerializeField] private Button _settingsButton;

        [Header("Levels")]
        [SerializeField] private LevelScrollRect _scrollRect;
        [SerializeField] private List<LevelPanel> _levelPanels = new();
        [SerializeField] private int _levelWidth = 400;
        [SerializeField] private int _spaceBetweenLevels = 84;
        [SerializeField] private int _totalLevels = 100;

        public override void Initialize()
        {
            base.Initialize();

            _presenter.OpenSettingsWindowCommand
                .BindTo(_settingsButton)
                .AddTo(Disposables);

            _presenter.OpenUpgradeWindowCommand
                .BindTo(_upgradeButton)
                .AddTo(Disposables);
        }

        public void InitializeLevelPanels(int currentLevel)
        {
            _scrollRect.Initialize(currentLevel, _levelPanels, _levelWidth, _spaceBetweenLevels, _totalLevels);
        }
    }
}