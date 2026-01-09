using _Project.Scripts.UI.Windows.BaseWindow;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using VContainer;

namespace _Redactor.Scripts.LevelRedactorWindow
{
    public class LevelRedactorWindowPresenter : BaseWindowPresenter
    {
        [Inject] private RedactorManager _redactorManager;

        [SerializeField] private LevelRedactorWindowModel _model;
        [SerializeField] private LevelRedactorWindowView _view;
        public override BaseWindowModel Model => _model;
        public override BaseWindowView View => _view;

        public ReactiveCommand<int> SaveLevelCommand { get; } = new();
        public ReactiveCommand<int> LoadLevelCommand { get; } = new();
        public ReactiveCommand<int> PlayLevelCommand { get; } = new();

        public override void Initialize()
        {
            base.Initialize();
            SaveLevelCommand.Subscribe(levelIndex => _redactorManager.SaveLevel(levelIndex)).AddTo(Disposables);
            LoadLevelCommand.Subscribe(levelIndex => _redactorManager.LoadLevel(levelIndex, false).Forget()).AddTo(Disposables);
            PlayLevelCommand.Subscribe(levelIndex => _redactorManager.StartLevel(levelIndex).Forget()).AddTo(Disposables);
        }
    }
}