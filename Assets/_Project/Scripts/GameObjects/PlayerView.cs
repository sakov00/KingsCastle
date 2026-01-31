using System;
using _Project.Scripts.GameObjects.Abstract.BaseObject;
using _Project.Scripts.GameObjects.Abstract.Unit;
using _Project.Scripts.UI.Info;
using UnityEngine;

namespace _Project.Scripts.GameObjects
{
    [Serializable]
    public class PlayerView : ObjectView
    {
        [SerializeField] private UniversalBar _loadBar;
        [SerializeField] private UniversalBar _ultimateBar;

        public override void Initialize()
        {
            base.Initialize();
        }
        
        public virtual void SetWalking(bool isWalking)
        {
        }

        public virtual void SetAttacking(bool isAttacking)
        {

        }

        public void UpdateLoadBar(float currentValue, float maxValue)
        {
            _loadBar.UpdateBar(currentValue, maxValue);
        }
        
        public void UpdateUltimateBar(float currentValue, float maxValue)
        {
            _ultimateBar.UpdateBar(currentValue, maxValue);
        }
    }
}