using System;
using _Project.Scripts.GameObjects.Abstract.Unit;
using _Project.Scripts.UI.Info;
using UnityEngine;

namespace _Project.Scripts.GameObjects
{
    [Serializable]
    public class PlayerView : UnitView
    {
        [SerializeField] private UniversalBar _loadBar;
        [SerializeField] private UniversalBar _ultimateBar;

        public override void Initialize()
        {
            base.Initialize();
        }
        
        public override void SetWalking(bool isWalking)
        {
        }

        public override void SetAttacking(bool isAttacking)
        {
            OnAttackHit();
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