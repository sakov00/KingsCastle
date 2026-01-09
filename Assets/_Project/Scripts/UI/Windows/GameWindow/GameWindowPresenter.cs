using System;
using System.Linq;
using _Project.Scripts.AllAppData;
using _Project.Scripts.GameObjects.Additional.EnemyRoads;
using _Project.Scripts.Registries;
using _Project.Scripts.UI.Windows.BaseWindow;
using _Project.Scripts.UI.Windows.FailWindow;
using _Project.Scripts.UI.Windows.PauseWindow;
using _Project.Scripts.UI.Windows.WinWindow;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using VContainer;

namespace _Project.Scripts.UI.Windows.GameWindow
{
    public class GameWindowPresenter : BaseWindowPresenter
    {
        [Inject] private AppData _appData;
        [Inject] private SaveRegistry _saveRegistry;
        [Inject] private GameManager _gameManager;
        
        private Action _winEventHandler;
        private Action _failEventHandler;
        
        [SerializeField] private GameWindowModel _model;
        [SerializeField] private GameWindowView _view;
        
        public override BaseWindowModel Model => _model;
        public override BaseWindowView View => _view;
        
        public ReactiveCommand OpenSettingsWindowCommand { get; } = new();
        public ReactiveCommand OpenPauseWindowCommand { get; } = new();
        public ReactiveCommand NextRoundCommand { get; } = new();
        public ReactiveCommand SetStrategyModeCommand { get; } = new();
        
        public IReactiveProperty<bool> IsStrategyMode => _model.IsStrategyModeReactive;

        public override void Initialize()
        {
            base.Initialize();
            OpenSettingsWindowCommand.Subscribe(_ => OpenSettingsWindow()).AddTo(Disposables);
            OpenPauseWindowCommand.Subscribe(_ => OpenPauseWindow()).AddTo(Disposables);
            SetStrategyModeCommand.Subscribe(_ => SetStrategyMode()).AddTo(Disposables);
            NextRoundCommand.Subscribe(_ => NextRoundOnClick()).AddTo(Disposables);
            
            _appData.LevelEvents.WinEvent += WinHandle;
            _appData.LevelEvents.FailEvent += FailHandle;
        }

        private void OpenPauseWindow() => WindowsManager.ShowWindow<PauseWindowPresenter>();
        private void OpenSettingsWindow() => WindowsManager.ShowWindow<PauseWindowPresenter>();
        private void SetStrategyMode() => _model.IsStrategyMode = !_model.IsStrategyMode;
        private void NextRoundOnClick()
        {
            _appData.LevelData.IsFighting = true;
            _appData.LevelData.ObjectsForRestoring = _saveRegistry.GetAll()
                .Select(o => o.GetSavableModel().GetSaveData()).ToList();
            
            _saveRegistry.GetAllByType<EnemyRoadController>().ForEach(x => x.StartSpawn());
            _appData.LevelEvents.Initialize();
        }

        private async UniTaskVoid WinHandle()
        {
            Dispose();
            _appData.LevelData.IsFighting = false;
            await WindowsManager.ShowWindow<WinWindowPresenter>();
            WindowsManager.HideFastWindow<GameWindowPresenter>();
        }

        private async UniTaskVoid FailHandle()
        {
            Dispose();
            _appData.LevelData.IsFighting = false;
            await WindowsManager.ShowWindow<FailWindowPresenter>();
            WindowsManager.HideFastWindow<GameWindowPresenter>();
        }

        public override void Dispose()
        {
            base.Dispose();
            _model.IsStrategyMode = false;
            _appData.LevelEvents.WinEvent -= WinHandle;
            _appData.LevelEvents.FailEvent -= FailHandle;
        }
    }
}