using _Project.Scripts._VContainer;
using _Project.Scripts.AllAppData;
using _Project.Scripts.Enums;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Registries;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace _Project.Scripts.GameObjects.Abstract.BaseObject
{
    public abstract class ObjectController<TModel, TView> : ObjectController
        where TModel : ObjectModel
        where TView : ObjectView 
    {
        protected TModel Model => (TModel)BaseModel;
        protected TView View => (TView)BaseView;

        protected virtual void FixedUpdate()
        {
            View.UpdateHealthBar(Model.CurrentHealth, Model.MaxHealth);
        }
    }

    public abstract class ObjectController : MonoBehaviour, ISavableController, IPoolableDispose, IKilled
    {
        [Inject] protected AppData AppData;
        [Inject] protected LiveRegistry LiveRegistry;
        [Inject] protected SaveRegistry SaveRegistry;
        
        [SerializeReference, SubclassSelector]
        private ObjectModel _model;
        
        [SerializeReference, SubclassSelector]
        private ObjectView _view;

        public WarSide WarSide => BaseModel.WarSide;

        protected ObjectModel BaseModel
        {
            get { return _model; } 
            set { _model = value; }
        }
    
        protected ObjectView BaseView
        {
            get { return _view; } 
            set { _view = value; }
        }
    
        protected virtual void Awake()
        {
            InjectManager.Inject(this);
        }
        
        public virtual UniTask InitializeAsync()
        {
            LiveRegistry.Register(this);
            SaveRegistry.Register(this);
            Dispose(false, false);
            return default;
        }
        
        private void OnDestroy()
        {
            Dispose(false);
        }
        
        public ISavableModel GetSavableModel() => _model;
        public void SetSavableModel(ISavableModel savableModel)
        {
            _model.SavePosition = transform.position;
            _model.SaveRotation = transform.rotation;
            _model = (ObjectModel)savableModel;
        }

        public abstract void Killed();
        public abstract void Dispose(bool returnToPool = true, bool clearFromRegistry = true);
    }
}