using _Project.Scripts._GlobalLogic;
using _Project.Scripts.AllAppData;
using _Project.Scripts.GameObjects.Abstract.Unit;
using _Project.Scripts.GameObjects.ActionSystems;
using _Project.Scripts.Interfaces;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using ISavableModel = _Project.Scripts.Interfaces.ISavableModel;

namespace _Project.Scripts.GameObjects.Concrete.Player
{
    public class PlayerController : UnitController, ISelectable
    {
        [Inject] private AppData _appData;
        [Inject] private GameTimer _gameTimer;
        [field: SerializeField] public PlayerModel Model { get; private set; }
        [field: SerializeField] public PlayerView View { get; private set; }
        protected override UnitModel UnitModel => Model;
        protected override UnitView UnitView => View;
        
        private DamageSystem _damageSystem;
        private PlayerMovementSystem _playerMovementSystem;
        private RegenerationHpSystem _regenerationHpSystem;

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
            
            _playerMovementSystem = new PlayerMovementSystem(Model, View, transform);
            _damageSystem = new DamageSystem(this, transform);
            _regenerationHpSystem = new RegenerationHpSystem(Model, View);

            return default;
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            _damageSystem?.Attack();
            View.UpdateUltimateBar(Model.CurrentValueUltimate, Model.MaxValueUltimate);
        }

        private void Update()
        {
            _playerMovementSystem?.MoveTo(_appData.LevelData.MoveDirection);
        }

        public override ISavableModel GetSavableModel()
        {
            Model.SavePosition = transform.position;
            Model.SaveRotation = transform.rotation;
            return Model;
        }

        public override void SetSavableModel(ISavableModel savableModel) =>
            Model.LoadData(savableModel);

        public void AddUltimateValue()
        {
            Model.CurrentValueUltimate += Model.ShootRewardValue;
            if (Model.IsActiveUltimate == false && Model.CurrentValueUltimate == Model.MaxValueUltimate)
            {
                Model.IsActiveUltimate = true;
                Model.DamageAmount *= Model.UltimateUpDamageModifier;
                _gameTimer.Subscribe(1f, DisableUltimate);
            }
        }

        private void DisableUltimate()
        {
            Model.CurrentTimeUltimate++;
            if (Model.CurrentTimeUltimate == Model.DurationUltimate)
            {
                Model.IsActiveUltimate = false;
                Model.DamageAmount = Model.DefaultDamageAmount;
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
                Model.DamageAmount = Model.DefaultDamageAmount;
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
            _playerMovementSystem = null;
            _regenerationHpSystem?.Dispose();
            _damageSystem?.Dispose();
        }
    }
}