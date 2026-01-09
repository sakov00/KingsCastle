using _Project.Scripts._GlobalLogic;
using _Project.Scripts.Factories;
using _Project.Scripts.GameObjects.Abstract;
using _Project.Scripts.GameObjects.Abstract.Unit;
using _Project.Scripts.GameObjects.Concrete.FriendsGroup;
using _Project.Scripts.Pools;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using VContainer;
using ISavableModel = _Project.Scripts.Interfaces.ISavableModel;

namespace _Project.Scripts.GameObjects.Concrete.FriendsBuild
{
    public class FriendsBuildController : BuildController
    {
        [Inject] private GameTimer _gameTimer;
        [Inject] private UnitPool _unitPool;
        [Inject] private UnitFactory _unitFactory;
        
        [field: SerializeField] public FriendsBuildModel Model { get; set; }
        [field: SerializeField] public FriendsBuildView View { get; set; }
        protected override BuildModel BuildModel => Model;
        protected override BuildView BuildView => View;
        
        private FriendsGroupController FriendsGroup { get; set; }

        public override UniTask InitializeAsync()
        {
            base.InitializeAsync();
            
            Model.CurrentHealth = Model.MaxHealth;

            SetFriendsGroup();
            
            View.Initialize();
            return default;
        }

        private void SetFriendsGroup()
        {
            if (Model.FriendsGroupId == 0)
            {
                FriendsGroup = _unitFactory.CreateFriendsGroup(Model.UnitType, View.GroupPoint.position);
                FriendsGroup.InitializeAsync();
                Model.FriendsGroupId = FriendsGroup.Id;
            }
            else
            {
                FriendsGroup = (FriendsGroupController)IdsRegistry.Get(Model.FriendsGroupId);
            }
            FriendsGroup.UnitOnKilled += CheckRemovedUnit;
        }

        private void CheckRemovedUnit(UnitController removedUnit)
        {
            Model.NeedRestoreUnitsCount++;

            if (Model.NeedRestoreUnitsCount > 0 && Model.CurrentSpawnTimer < 0)
            {
                Model.CurrentSpawnTimer = 0;
                _gameTimer.Subscribe(1f, HandleSpawnTimer);
            }
        }

        private void HandleSpawnTimer()
        {
            Model.CurrentSpawnTimer++;
            View.UpdateLoadBar(Model.CurrentSpawnTimer, Model.TimeCreateUnits);

            if (Model.CurrentSpawnTimer < Model.TimeCreateUnits) return;

            if (Model.NeedRestoreUnitsCount > 0)
            {
                var unitController = _unitPool.Get(Model.UnitType, View.SpawnPoint.position);
                unitController.InitializeAsync();
                FriendsGroup.Units.Add(unitController);
                Model.NeedRestoreUnitsCount--;
            }

            if (Model.NeedRestoreUnitsCount > 0)
            {
                Model.CurrentSpawnTimer = 0;
            }
            else
            {
                _gameTimer.Unsubscribe(HandleSpawnTimer);
                Model.CurrentSpawnTimer = -1;
            }
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
            if (returnToPool)
            {
                Model.FriendsGroupId = 0;
            }
            _gameTimer.Unsubscribe(HandleSpawnTimer);
            if(FriendsGroup != null) FriendsGroup.UnitOnKilled -= CheckRemovedUnit;
        }
    }
}