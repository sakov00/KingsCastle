using System;
using _Project.Scripts._VContainer;
using _Project.Scripts.AllAppData;
using _Project.Scripts.Enums;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Registries;
using Cysharp.Threading.Tasks;
using UniRx.Toolkit;
using UnityEngine;
using VContainer;
using ISavableModel = _Project.Scripts.Interfaces.ISavableModel;

namespace _Project.Scripts.GameObjects.Abstract.BaseObject
{
    public abstract class ObjectController : MonoBehaviour, ISavableController, IPoolableDispose, IId, IKilled
    {
        [Inject] protected AppData AppData;
        [Inject] protected IdsRegistry IdsRegistry;
        [Inject] protected LiveRegistry LiveRegistry;
        [Inject] protected SaveRegistry SaveRegistry;
        
        protected abstract ObjectModel ObjectModel { get; }
        protected abstract ObjectView ObjectView { get; }
        
        public int Id { get => ObjectModel.Id; set => ObjectModel.Id = value; }
        public float HeightObject { get; set; }
        public WarSide WarSide => ObjectModel.WarSide;
        public float CurrentHealth { get => ObjectModel.CurrentHealth; set => ObjectModel.CurrentHealth = value; }

        private void Awake()
        {
            HeightObject = ObjectView.GetHeightObject();
            InjectManager.Inject(this);
        }

        protected virtual void FixedUpdate()
        {
            ObjectView.UpdateHealthBar(ObjectModel.CurrentHealth, ObjectModel.MaxHealth);
        }
        
        private void OnDestroy()
        {
            Dispose(false);
        }
        
        public virtual UniTask InitializeAsync()
        {
            IdsRegistry.Register(this);
            LiveRegistry.Register(this);
            SaveRegistry.Register(this);
            Dispose(false, false);
            return default;
        }
        
        public abstract ISavableModel GetSavableModel();
        public abstract void SetSavableModel(ISavableModel model);
        public abstract void Killed();
        public virtual void Dispose(bool returnToPool = true, bool clearFromRegistry = true)
        {
            if (returnToPool)
            {
                ObjectModel.CurrentHealth = ObjectModel.MaxHealth;
            }
            if (clearFromRegistry)
            {
                IdsRegistry.Unregister(this);
                LiveRegistry.Unregister(this);
                SaveRegistry.Unregister(this);
            }
        }
    }
}