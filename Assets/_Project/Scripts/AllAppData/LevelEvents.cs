using System;
using System.Linq;
using _Project.Scripts.Enums;
using _Project.Scripts.GameObjects.Abstract;
using _Project.Scripts.GameObjects.Abstract.BaseObject;
using _Project.Scripts.GameObjects.Abstract.Unit;
using _Project.Scripts.Registries;
using Cysharp.Threading.Tasks;
using UniRx;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts.AllAppData
{
    public class LevelEvents : IInitializable
    {
        [Inject] private LiveRegistry _liveRegistry;

        public event Func<UniTaskVoid> WinEvent;
        public event Func<UniTaskVoid> FailEvent;

        private CompositeDisposable _disposables;

        public void Initialize()
        {
            _liveRegistry.OnRemoveAsObservable()
                .Subscribe(removedObject => TryInvokeAllEnemiesKilled(removedObject.Value));
            
            _liveRegistry.OnRemoveAsObservable()
                .Subscribe(removedObject => TryInvokeMainBuildDestroyed(removedObject.Value));
        }

        private void TryInvokeAllEnemiesKilled(ObjectController objectController)
        {
            if (objectController.WarSide == WarSide.Enemy &&
                _liveRegistry.GetAllByType<UnitController>().All(x => x.WarSide != WarSide.Enemy))
                WinEvent?.Invoke();
        }
        
        private void TryInvokeMainBuildDestroyed(ObjectController objectController)
        {
            if (objectController is BuildController build && build.BuildType == BuildType.MainBuild)
                FailEvent?.Invoke();
        }
    }
}