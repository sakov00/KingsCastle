using _Project.Scripts.GameObjects.Abstract;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using ISavableModel = _Project.Scripts.Interfaces.ISavableModel;

namespace _Project.Scripts.GameObjects.Concrete.MoneyBuild
{
    public class MoneyBuildController : BuildController
    {
        [Inject] private GameManager _gameManager;
        [field: SerializeField] public MoneyBuildModel Model { get; private set; }
        [field: SerializeField] public MoneyBuildingView View { get; private set; }
        protected override BuildModel BuildModel => Model;
        protected override BuildView BuildView => View;
        
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

        public override ISavableModel GetSavableModel()
        {
            Model.SavePosition = transform.position;
            Model.SaveRotation = transform.rotation;
            return Model;
        }

        public override void SetSavableModel(ISavableModel savableModel) =>
            Model.LoadData(savableModel);
        
        public override void Dispose(bool returnToPool = true, bool clearFromRegistry = true)
        {
            base.Dispose(returnToPool, clearFromRegistry);
            _gameManager.WinEvent -= AddMoneyToPlayer;
        }
    }
}