using _Project.Scripts.GameObjects.Abstract.BaseObject;
using UnityEngine;

namespace _Project.Scripts.GameObjects.Features
{
    public class VisibilityForwarder : MonoBehaviour
    {
        [SerializeField] private ObjectController _objectController;
        private void OnBecameVisible()
        {
            _objectController.IsVisible = true;
        }

        private void OnBecameInvisible()
        {
            _objectController.IsVisible = false;
        }
    }
}