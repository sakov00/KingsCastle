using _Project.Scripts.Enums;
using _Project.Scripts.GameObjects.Abstract.Unit;
using _Project.Scripts.ServicesGameplay;
using Cysharp.Threading.Tasks;
using UnityEngine;
using ISavableModel = _Project.Scripts.Interfaces.ISavableModel;

namespace _Project.Scripts.GameObjects
{
    public class WarriorController : UnitController<WarriorModel, WarriorView>
    {
        private AttackService _attackService;
        private UnitMovementService _unitMovementService;

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            _unitMovementService?.MoveToAim();
            _attackService?.Attack();
        }

        public override UniTask InitializeAsync()
        {
            base.InitializeAsync();
            
            Model.CurrentHealth = Model.MaxHealth;
            
            _unitMovementService = new UnitMovementService(Model, View, transform);
            // _attackService = new AttackService(this, transform);
            View.Initialize();
            return default;
        }
        
        public override void Dispose(bool returnToPool = true, bool clearFromRegistry = true)
        {
            base.Dispose(returnToPool, clearFromRegistry);
            _attackService?.Dispose();
        }
    }
}