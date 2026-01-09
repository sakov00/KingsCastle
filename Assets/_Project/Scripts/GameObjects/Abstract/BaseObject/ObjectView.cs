using System;
using _Project.Scripts.UI.Info;
using QuickOutline.Scripts;
using UnityEngine;

namespace _Project.Scripts.GameObjects.Abstract.BaseObject
{
    public abstract class ObjectView : MonoBehaviour
    {
        [SerializeField] protected Renderer _objRenderer;
        [SerializeField] protected UniversalBar _healthBar;
        [SerializeField] protected Tooltip _tooltip;
        [SerializeField] protected Outline _outline;

        public virtual void Initialize()
        {
            transform.SetParent(null);
            gameObject.SetActive(true);
        }

        public void UpdateHealthBar(float currentHealth, float maxHealth)
        {
            _healthBar.UpdateBar(currentHealth, maxHealth);
        }

        public void UpdateTooltip(int currentLvl, int? costUpgrade)
        {
            _tooltip.UpdateTooltip(currentLvl, costUpgrade);
        }

        public float GetHeightObject()
        {
            return _objRenderer.bounds.size.y;
        }
        
        public void EnableOutline(bool enable)
        {
            if (_outline != null)
                _outline.enabled = enable;
        }
    }
}