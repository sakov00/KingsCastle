using System;
using System.Collections.Generic;
using _Project.Scripts.Enums;
using _Project.Scripts.GameObjects.Abstract.BaseObject;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Pools;
using UnityEngine;
using VContainer;

namespace _Project.Scripts.GameObjects.Abstract.Unit
{
    public abstract class UnitController<TModel, TView> : UnitController
        where TModel : UnitModel
        where TView : UnitView 
    {
        protected new TModel Model => (TModel)base.Model;
        protected new TView View => (TView)base.View;
    }
    
    public abstract class UnitController : ObjectController<UnitModel, UnitView>
    {
        [Inject] protected UnitPool UnitPool;

        public Action<UnitController> OnKilled;
        public UnitType UnitType => Model.UnitType;
        
        public void SetWayToPoint(List<Vector3> waypoints)
        {
            Model.WayToAim = waypoints;
        }
        
        public void Select()
        {
            View.EnableOutline(true);
        }

        public void Deselect()
        {
            View.EnableOutline(false);
        }

        public void MoveTo(Vector3 position)
        {
            View.Agent.enabled = false;
            transform.position = position;
            View.Agent.enabled = true;
        }

        public override void Killed()
        {
            Dispose();
        }
        
        public override void Dispose(bool returnToPool = true, bool clearFromRegistry = true)
        {
            if (returnToPool)
            {
                UnitPool.Return(this);
                OnKilled?.Invoke(this);
                OnKilled = null;
                Model.AimObject = null;
            }
            if (clearFromRegistry)
            {
                LiveRegistry.Unregister(this);
                SaveRegistry.Unregister(this);
            }
        }
    }
}