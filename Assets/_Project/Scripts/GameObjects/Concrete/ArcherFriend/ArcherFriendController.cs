using _Project.Scripts.GameObjects.Abstract.Unit;
using _Project.Scripts.GameObjects.ActionSystems;
using _Project.Scripts.Interfaces;
using Cysharp.Threading.Tasks;
using UnityEngine;
using ISavableModel = _Project.Scripts.Interfaces.ISavableModel;

namespace _Project.Scripts.GameObjects.Concrete.ArcherFriend
{
    public class ArcherFriendController : UnitController
    {
        [field: SerializeField] public ArcherFriendModel Model { get; private set; }
        [field: SerializeField] public ArcherFriendView View { get; private set; }
        protected override UnitModel UnitModel => Model;
        protected override UnitView UnitView => View;
        
        private DamageSystem _damageSystem;
        private RegenerationHpSystem _regenerationHpSystem;
        private UnitMovementSystem _unitMovementSystem;

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            _unitMovementSystem?.MoveToAim();
            _damageSystem?.Attack();
        }

        public override UniTask InitializeAsync()
        {
            base.InitializeAsync();
            
            Model.CurrentHealth = Model.MaxHealth;
            
            _unitMovementSystem = new UnitMovementSystem(Model, View, transform);
            _damageSystem = new DamageSystem(this, transform);
            _regenerationHpSystem = new RegenerationHpSystem(Model, View);
            
            View.Initialize();
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
            _regenerationHpSystem?.Dispose();
            _damageSystem?.Dispose();
        }
    }
}