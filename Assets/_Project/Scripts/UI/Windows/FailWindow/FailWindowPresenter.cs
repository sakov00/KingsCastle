using _Project.Scripts.AllAppData;
using _Project.Scripts.UI.Windows.BaseWindow;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using VContainer;

namespace _Project.Scripts.UI.Windows.FailWindow
{
    public class FailWindowPresenter : BaseWindowPresenter
    {
        [Inject] private AppData _appData;
        [Inject] private GameManager _gameManager;
        
        [SerializeField] private FailWindowModel _model;
        [SerializeField] private FailWindowView _view;
        public override BaseWindowModel Model => _model;
        public override BaseWindowView View => _view;
        
        public ReactiveCommand HomeCommand { get; } = new();
        public ReactiveCommand RestartCommand { get; } = new();
        
        public override void Initialize()
        {
            base.Initialize();
            HomeCommand.Subscribe(_ => HomeOnClick()).AddTo(Disposables);
            RestartCommand.Subscribe(_ => RestartOnClick().Forget()).AddTo(Disposables);
        }
        
        private void HomeOnClick()
        {
        }
        
        private async UniTaskVoid RestartOnClick()
        {
            await WindowsManager.HideWindow<FailWindowPresenter>();
            await _gameManager.RestartLevel();
        }
    }
}