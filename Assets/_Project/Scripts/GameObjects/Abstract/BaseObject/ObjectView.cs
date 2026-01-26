using _Project.Scripts.UI.Info;
using UnityEngine;

namespace _Project.Scripts.GameObjects.Abstract.BaseObject
{
    public abstract class ObjectView
    {
        [SerializeField] protected Transform _transform;
        [SerializeField] protected Renderer _objRenderer;
        [SerializeField] protected UniversalBar _healthBar;
        [SerializeField] protected Outline _outline;

        public virtual void Initialize()
        {
            _transform.SetParent(null);
            _transform.gameObject.SetActive(true);
        }

        public void UpdateHealthBar(float currentHealth, float maxHealth)
        {
            _healthBar?.UpdateBar(currentHealth, maxHealth);
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