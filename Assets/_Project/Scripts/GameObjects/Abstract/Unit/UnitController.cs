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
    public abstract class UnitController : ObjectController, IFightController
    {
        [Inject] protected UnitPool UnitPool;

        protected abstract UnitModel UnitModel { get; }
        protected abstract UnitView UnitView { get; }
        protected override ObjectModel ObjectModel => UnitModel;
        protected override ObjectView ObjectView => UnitView;
        public IFightModel FightModel => UnitModel;
        public IFightView FightView => UnitView;
        
        public Action<UnitController> OnKilled;
        public UnitType UnitType => UnitModel.UnitType;

        public void SetWayToPoint(List<Vector3> waypoints)
        {
            UnitModel.WayToAim = waypoints;
        }
        
        public void Select()
        {
            UnitView.EnableOutline(true);
        }

        public void Deselect()
        {
            UnitView.EnableOutline(false);
        }

        public void MoveTo(Vector3 position)
        {
            UnitView.Agent.enabled = false;
            transform.position = position;
            UnitView.Agent.enabled = true;
        }

        public override void Killed()
        {
            Dispose();
        }

        public override void Dispose(bool returnToPool = true, bool clearFromRegistry = true)
        {
            base.Dispose(returnToPool, clearFromRegistry);
            if (returnToPool)
            {
                UnitPool.Return(this);
                OnKilled?.Invoke(this);
                OnKilled = null;
                UnitModel.AimObject = null;
            }
        }
    }
}