using System;
using System.Linq;
using _Project.Scripts._GlobalLogic;
using _Project.Scripts.AllAppData;
using _Project.Scripts.Enums;
using _Project.Scripts.GameObjects.Additional.EnemyRoads;
using _Project.Scripts.Registries;
using _Project.Scripts.Services;
using _Project.Scripts.UI.WindowElements;
using Cysharp.Threading.Tasks;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace _Project.Scripts.UI.Windows.GameWindow
{
    public class GameWindow : BaseWindow
    {
        [Inject] private AppData _appData;
        [Inject] private SoundManager _soundManager;
        [Inject] private SaveRegistry _saveRegistry;
        [Inject] private GameManager _gameManager;

        [Header("Buttons")]
        [SerializeField] private Button _openPauseMenuButton;
        [SerializeField] private Button _nextRoundButton;
        [SerializeField] private Button _strategyModeButton;

        [Header("UI Text")]
        [SerializeField] private TextMeshProUGUI _moneyText;
        [SerializeField] private TextMeshProUGUI _currentRoundText;

        [Header("Controls")]
        [SerializeField] private Joystick _joystick;
        [SerializeField] private TouchAndMouseDragInput _touchAndMouseDragInput;
        
        private const string MoneyFormat = "Money: {0}";
        private const string RoundFormat = "Round: {0}";

        protected override void Awake()
        {
            base.Awake();
            
            _appData.LevelEvents.WinEvent += WinHandle;
            _appData.LevelEvents.FailEvent += FailHandle;
        }

        public override void Initialize()
        {
            base.Initialize();
            
            _appData.LevelData.IsStrategyModeReactive
                .Subscribe(async isStrategy =>
                {
                    if (isStrategy)
                    {
                        _touchAndMouseDragInput.gameObject.SetActive(true);
                        _joystick.gameObject.SetActive(false);
                        GlobalObjects.CameraController.CameraFollow.DisableFollowAnimation();
                    }
                    else
                    {
                        _touchAndMouseDragInput.gameObject.SetActive(false);
                        await GlobalObjects.CameraController.CameraFollow.EnableFollowAnimation();
                        _joystick.gameObject.SetActive(true);
                    }
                })
                .AddTo(Disposables);
            
            _openPauseMenuButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    _soundManager.PlaySFX(SoundKey.ButtonClickSound);
                })
                .AddTo(Disposables);

            _nextRoundButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    _soundManager.PlaySFX(SoundKey.ButtonClickSound);
                    _soundManager.PlayMusicAsync(SoundKey.BattleMusic).Forget();
                })
                .AddTo(Disposables);

            _strategyModeButton.OnClickAsObservable()
                .Subscribe(_ => _soundManager.PlaySFX(SoundKey.ButtonClickSound))
                .AddTo(Disposables);
            
            _appData.LevelData.IsFightingReactive
                .Subscribe(roundActive => _nextRoundButton.gameObject.SetActive(!roundActive))
                .AddTo(Disposables);
            
            _appData.LevelData.LevelMoneyReactive
                .Subscribe(money => _moneyText.text = string.Format(MoneyFormat, money))
                .AddTo(Disposables);
            
            _appData.LevelData.CurrentRoundReactive
                .Subscribe(roundIndex => _currentRoundText.text = string.Format(RoundFormat, roundIndex + 1))
                .AddTo(Disposables);
        }
        
        private void OpenPauseWindow() => WindowsManager.ShowWindow<PauseWindow>();
        private void OpenSettingsWindow() => WindowsManager.ShowWindow<PauseWindow>();
        private void SetStrategyMode() => _appData.LevelData.IsStrategyMode = !_appData.LevelData.IsStrategyMode;
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
            await WindowsManager.ShowWindow<WinWindow>();
            WindowsManager.HideFastWindow<GameWindow>();
        }

        private async UniTaskVoid FailHandle()
        {
            Dispose();
            _appData.LevelData.IsFighting = false;
            await WindowsManager.ShowWindow<FailWindow>();
            WindowsManager.HideFastWindow<GameWindow>();
        }

        public override void Dispose()
        {
            base.Dispose();
            _appData.LevelData.IsStrategyMode = false;
            _appData.LevelEvents.WinEvent -= WinHandle;
            _appData.LevelEvents.FailEvent -= FailHandle;
        }
    }
}