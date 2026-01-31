using _Project.Scripts._GlobalLogic;
using _Project.Scripts.AllAppData;
using _Project.Scripts.GameObjects.Abstract.BaseObject;
using _Project.Scripts.GameObjects.Abstract.Unit;
using _Project.Scripts.ServicesGameplay;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace _Project.Scripts.GameObjects
{
    public class PlayerController : ObjectController<PlayerModel, PlayerView>
    {
        [Inject] private AppData _appData;
        [Inject] private GameTimer _gameTimer;
        
        private PlayerMovementService _playerMovementService;
        private RegenerationHpService _regenerationHpService;

        public override void Initialize()
        {
            base.Initialize();
            
            View.Initialize();
            View.UpdateLoadBar(Model.CurrentTimeResurrection, Model.DurationTimeResurrection);
            
            if (Model.IsActiveUltimate)
                _gameTimer.Subscribe(1f, DisableUltimate);
            
            if (Model.IsKilled)
            {
                Killed();
            }

            if(Model.IsNoDamageable)
                _gameTimer.Subscribe(1f, DisableNoDamage);
            
            _regenerationHpService = new RegenerationHpService(Model, View);

        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            View.UpdateUltimateBar(Model.CurrentValueUltimate, Model.MaxValueUltimate);
        }

        private void Update()
        {
            _playerMovementService?.MoveTo(_appData.LevelData.MoveDirection);
        }
        
        

        public void AddUltimateValue()
        {
            Model.CurrentValueUltimate += Model.ShootAddUltimate;
            if (Model.IsActiveUltimate == false && Model.CurrentValueUltimate == Model.MaxValueUltimate)
            {
                Model.IsActiveUltimate = true;
                // Model.DamageAmount *= Model.UltimateUpDamageModifier;
                _gameTimer.Subscribe(1f, DisableUltimate);
            }
        }

        private void DisableUltimate()
        {
            // Model.CurrentTimeUltimate++;
            // if (Model.CurrentTimeUltimate == Model.DurationUltimate)
            // {
            //     Model.IsActiveUltimate = false;
            //     // Model.DamageAmount = Model.DefaultDamageAmount;
            //     Model.CurrentValueUltimate = 0;
            //     Model.CurrentTimeUltimate = 0;
            //     _gameTimer.Unsubscribe(DisableUltimate);
            // }
        }

        public override async UniTask Killed(Vector3 forceDirection = default, float forceAmount = 0f)
        {
            await UniTask.Delay(1);
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
                Initialize();
                Model.IsNoDamageable = true;
                _gameTimer.Unsubscribe(TryReturnToGame);
                _gameTimer.Subscribe(1f, DisableNoDamage);
            }
        }
        
        private void DisableNoDamage()
        {
            Model.CurrentTimeNoDamage++;
            if (Model.CurrentTimeNoDamage == Model.SecondsNoDamage)
            {
                _gameTimer.Unsubscribe(DisableNoDamage);
                Model.CurrentTimeNoDamage = 0;
                Model.IsNoDamageable = false;
            }
        }

        public override void Dispose(bool returnToPool = true, bool clearFromRegistry = true)
        {
            if (returnToPool)
            {
                // Model.DamageAmount = Model.DefaultDamageAmount;
                Model.IsActiveUltimate = false;
                Model.IsKilled = false;
                Model.IsNoDamageable = false;
                Model.CurrentValueUltimate = 0;
                // Model.CurrentTimeUltimate = 0;
                Model.CurrentTimeResurrection = 0;
                Model.CurrentTimeNoDamage = 0;
            }
            _gameTimer.Unsubscribe(DisableUltimate);
            _gameTimer.Unsubscribe(DisableNoDamage);
            _gameTimer.Unsubscribe(TryReturnToGame);
            _playerMovementService = null;
            _regenerationHpService?.Dispose();
        }
    }
}