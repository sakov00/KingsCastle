using _Project.Scripts._GlobalLogic;
using _Project.Scripts.AllAppData;
using _Project.Scripts.Enums;
using _Project.Scripts.Services;
using _Project.Scripts.UI.WindowElements;
using _Project.Scripts.UI.Windows.BaseWindow;
using Cysharp.Threading.Tasks;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace _Project.Scripts.UI.Windows.GameWindow
{
    public class GameWindowView : BaseWindowView
    {
        [Inject] private AppData _appData;
        [Inject] private SoundManager _soundManager;
        
        [Header("Presenter")]
        [SerializeField] private GameWindowPresenter _presenter;

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

        public override void Initialize()
        {
            base.Initialize();
            
            _presenter.OpenPauseWindowCommand.BindTo(_openPauseMenuButton).AddTo(Disposables);
            _presenter.NextRoundCommand.BindTo(_nextRoundButton).AddTo(Disposables);
            _presenter.SetStrategyModeCommand.BindTo(_strategyModeButton).AddTo(Disposables);
            
            _presenter.IsStrategyMode
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
                .Subscribe(_ => _soundManager.PlaySFX(SoundKey.ButtonClickSound))
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
    }
}