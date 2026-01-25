using _Project.Scripts._GlobalLogic;
using _Project.Scripts.AllAppData;
using _Project.Scripts.GameObjects.Abstract.Unit;
using _Project.Scripts.Interfaces;
using _Project.Scripts.ServicesGameplay;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using ISavableModel = _Project.Scripts.Interfaces.ISavableModel;

namespace _Project.Scripts.GameObjects
{
    public class PlayerController : UnitController<PlayerModel, PlayerView>
    {
        [Inject] private AppData _appData;
        [Inject] private GameTimer _gameTimer;
        
        private AttackService _attackService;
        private PlayerMovementService _playerMovementService;
        private RegenerationHpService _regenerationHpService;

        public override UniTask InitializeAsync()
        {
            base.InitializeAsync();
            
            View.Initialize();
            View.UpdateLoadBar(Model.CurrentTimeResurrection, Model.DurationTimeResurrection);
            
            if (Model.IsActiveUltimate)
                _gameTimer.Subscribe(1f, DisableUltimate);
            
            if (Model.IsKilled)
            {
                Killed();
                return default;
            }

            if(Model.IsNoDamageable)
                _gameTimer.Subscribe(1f, DisableNoDamage);
            
            _playerMovementService = new PlayerMovementService(Model, View, transform);
            // _attackService = new AttackService(this, transform);
            _regenerationHpService = new RegenerationHpService(Model, View);

            return default;
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            _attackService?.Attack();
            View.UpdateUltimateBar(Model.CurrentValueUltimate, Model.MaxValueUltimate);
        }

        private void Update()
        {
            _playerMovementService?.MoveTo(_appData.LevelData.MoveDirection);
        }

        public void AddUltimateValue()
        {
            Model.CurrentValueUltimate += Model.ShootRewardValue;
            if (Model.IsActiveUltimate == false && Model.CurrentValueUltimate == Model.MaxValueUltimate)
            {
                Model.IsActiveUltimate = true;
                // Model.DamageAmount *= Model.UltimateUpDamageModifier;
                _gameTimer.Subscribe(1f, DisableUltimate);
            }
        }

        private void DisableUltimate()
        {
            Model.CurrentTimeUltimate++;
            if (Model.CurrentTimeUltimate == Model.DurationUltimate)
            {
                Model.IsActiveUltimate = false;
                // Model.DamageAmount = Model.DefaultDamageAmount;
                Model.CurrentValueUltimate = 0;
                Model.CurrentTimeUltimate = 0;
                _gameTimer.Unsubscribe(DisableUltimate);
            }
        }

        public override void Killed()
        {
            Dispose(false,false);
            LiveRegistry.Unregister(this);
            Model.IsKilled = true;
            _gameTimer.Subscribe(1f, TryReturnToGame);
        }

        private void TryReturnToGame()
        {
            Model.CurrentTimeResurrection++;
            View.UpdateLoadBar(Model.CurrentTimeResurrection, Model.DurationTimeResurrection);
            if (Model.CurrentTimeResurrection == Model.DurationTimeResurrection)
            {
                Model.CurrentTimeResurrection = 0;
                Model.CurrentHealth = Model.MaxHealth;
                Model.IsKilled = false;
                InitializeAsync();
                Model.IsNoDamageable = true;
                _gameTimer.Unsubscribe(TryReturnToGame);
                _gameTimer.Subscribe(1f, DisableNoDamage);
            }
        }
        
        private void DisableNoDamage()
        {
            Model.CurrentTimeNoDamage++;
            if (Model.CurrentTimeNoDamage == Model.DurationTimeNoDamage)
            {
                _gameTimer.Unsubscribe(DisableNoDamage);
                Model.CurrentTimeNoDamage = 0;
                Model.IsNoDamageable = false;
            }
        }

        public override void Dispose(bool returnToPool = true, bool clearFromRegistry = true)
        {
            base.Dispose(returnToPool, clearFromRegistry);
            if (returnToPool)
            {
                // Model.DamageAmount = Model.DefaultDamageAmount;
                Model.IsActiveUltimate = false;
                Model.IsKilled = false;
                Model.IsNoDamageable = false;
                Model.CurrentValueUltimate = 0;
                Model.CurrentTimeUltimate = 0;
                Model.CurrentTimeResurrection = 0;
                Model.CurrentTimeNoDamage = 0;
            }
            _gameTimer.Unsubscribe(DisableUltimate);
            _gameTimer.Unsubscribe(DisableNoDamage);
            _gameTimer.Unsubscribe(TryReturnToGame);
            _playerMovementService = null;
            _regenerationHpService?.Dispose();
            _attackService?.Dispose();
        }
    }
}