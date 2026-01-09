using _Project.Scripts.Interfaces;
using _Project.Scripts.Pools;
using _Project.Scripts.Registries;
using Cysharp.Threading.Tasks;
using VContainer;

namespace _Project.Scripts.Services
{
    public class ResetLevelService
    {
        [Inject] private UnitPool _unitPool;
        [Inject] private IdsRegistry _idsRegistry;
        [Inject] private LiveRegistry _liveRegistry;
        [Inject] private SaveRegistry _saveRegistry;
        
        public async UniTask ResetLevel()
        {
            foreach (var obj in _saveRegistry.GetAllByType<IDestroyable>())
            {
                obj.Destroy();
                await UniTask.Yield();
            }

            foreach (var obj in _saveRegistry.GetAllByType<IPoolableDispose>())
                obj.Dispose();
            
            _idsRegistry.Clear();
            _liveRegistry.Clear();
            _saveRegistry.Clear();
        }
    }
}