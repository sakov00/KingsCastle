using UnityEngine;

namespace _Project.Animation.CameraLogic
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        [field:SerializeField] public CameraFollow CameraFollow { get; set; }
        [field:SerializeField] public Camera CurrentCamera { get; set; }

        private void OnValidate()
        {
            CurrentCamera ??= new Camera();
        }

        private void LateUpdate()
        {
            CameraFollow.UpdateCameraPosition();
        }
    }
}