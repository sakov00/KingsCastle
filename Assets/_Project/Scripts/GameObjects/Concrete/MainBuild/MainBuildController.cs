using _Project.Scripts.GameObjects.Abstract;
using _Project.Scripts.GameObjects.Abstract.Unit;
using _Project.Scripts.GameObjects.ActionSystems;
using _Project.Scripts.Interfaces;
using Cysharp.Threading.Tasks;
using UnityEngine;
using ISavableModel = _Project.Scripts.Interfaces.ISavableModel;

namespace _Project.Scripts.GameObjects.Concrete.MainBuild
{
    public class MainBuildController : BuildController, IFightController
    {
        [field: SerializeField] public MainBuildModel Model { get; private set; }
        [field: SerializeField] public MainBuildingView View { get; private set; }
        protected override BuildModel BuildModel => Model;
        protected override BuildView BuildView => View;

        public IFightModel FightModel => Model;
        public IFightView FightView => View;
        
        private DamageSystem _damageSystem;
        private float _heightObject;

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            _damageSystem?.Attack();
        }

        public override UniTask InitializeAsync()
        {
            base.InitializeAsync();
            
            Model.CurrentHealth = Model.MaxHealth;
            
            _damageSystem = new DamageSystem(this, transform);
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
            _damageSystem?.Dispose();
            Model.AimObject = null;
        }
    }
}