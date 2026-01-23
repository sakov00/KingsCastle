using _Project.Scripts.GameObjects.Abstract.Build;
using _Project.Scripts.Interfaces;
using _Project.Scripts.ServicesGameplay;
using Cysharp.Threading.Tasks;
using UnityEngine;
using BuildModel = _Project.Scripts.GameObjects.Abstract.Build.BuildModel;
using ISavableModel = _Project.Scripts.Interfaces.ISavableModel;

namespace _Project.Scripts.GameObjects
{
    public class MainBuildController : BuildController<MainBuildModel, MainBuildingView>
    {
        private AttackService _attackService;
        
        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            _attackService?.Attack();
        }

        public override UniTask InitializeAsync()
        {
            base.InitializeAsync();
            
            Model.CurrentHealth = Model.MaxHealth;
            
            // _attackService = new AttackService(this, transform);
            View.Initialize();
            return default;
        }
        
        public override void Dispose(bool returnToPool = true, bool clearFromRegistry = true)
        {
            base.Dispose(returnToPool, clearFromRegistry);
            _attackService?.Dispose();
            Model.AimObject = null;
        }
    }
}