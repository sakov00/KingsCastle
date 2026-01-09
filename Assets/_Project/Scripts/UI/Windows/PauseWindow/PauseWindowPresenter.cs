using _Project.Scripts.AllAppData;
using _Project.Scripts.UI.Windows.BaseWindow;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using VContainer;

namespace _Project.Scripts.UI.Windows.PauseWindow
{
    public class PauseWindowPresenter : BaseWindowPresenter
    {
        [Inject] private AppData _appData;
        [Inject] private GameManager _gameManager;
        
        [SerializeField] private PauseWindowModel _model;
        [SerializeField] private PauseWindowView _view;
        
        public override BaseWindowModel Model => _model;
        public override BaseWindowView View => _view;
        
        public ReactiveCommand HomeCommand { get; } = new();
        public ReactiveCommand RestartCommand { get; } = new();
        public ReactiveCommand ContinueCommand { get; } = new();

        public override void Initialize()
        {
            base.Initialize();
            HomeCommand.Subscribe(_ => HomeOnClick()).AddTo(Disposables);
            RestartCommand.Subscribe(_ => RestartOnClick().Forget()).AddTo(Disposables);
            ContinueCommand.Subscribe(_ => ContinueOnClick()).AddTo(Disposables);
        }
        
        private void HomeOnClick()
        {
        }
        
        private async UniTaskVoid RestartOnClick()
        {
            await WindowsManager.HideWindow<PauseWindowPresenter>();
            await _gameManager.RestartLevel();
        }
        
        private void ContinueOnClick()
        {
            WindowsManager.HideWindow<PauseWindowPresenter>();
        }
    }
}