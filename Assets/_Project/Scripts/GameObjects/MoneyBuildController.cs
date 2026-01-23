using _Project.Scripts.GameObjects.Abstract.Build;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using BuildModel = _Project.Scripts.GameObjects.Abstract.Build.BuildModel;
using ISavableModel = _Project.Scripts.Interfaces.ISavableModel;

namespace _Project.Scripts.GameObjects
{
    public class MoneyBuildController : BuildController<MoneyBuildModel, MoneyBuildingView>
    {
        [Inject] private GameManager _gameManager;
        
        public override UniTask InitializeAsync()
        {
            base.InitializeAsync();
            
            Model.CurrentHealth = Model.MaxHealth;
            
            View.Initialize();
            _gameManager.WinEvent += AddMoneyToPlayer;
            return default;
        }

        private UniTaskVoid AddMoneyToPlayer()
        {
            var moneyAmount = Model.AddMoneyValue;
            AppData.LevelData.LevelMoney += moneyAmount;
            View.MoneyUp(moneyAmount);
            return default;
        }
        
        public override void Dispose(bool returnToPool = true, bool clearFromRegistry = true)
        {
            base.Dispose(returnToPool, clearFromRegistry);
            _gameManager.WinEvent -= AddMoneyToPlayer;
        }
    }
}