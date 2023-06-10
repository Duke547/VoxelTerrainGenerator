using UnityEngine;
using UnityEngine.InputSystem;

namespace VoxelWorld.Scripts
{
    [RequireComponent(typeof(Camera))]
    public class PlayerCamera : MonoBehaviour
    {
        Vector3   cameraRotation;
        Transform originalParent;
        Vector3   originalLocalPosition;

        [Min(0)]
        public float mouseSensitivity = 0.2f;

        private void Start()
        {
            originalParent        = transform.parent;
            originalLocalPosition = transform.localPosition;

            transform.parent = null;
        }

        void UpdatePosition()
        {
            var targetPosition = originalParent.transform.position + originalLocalPosition;
            
            transform.position = Vector3.Lerp(transform.position, targetPosition, 10 * Time.deltaTime);
        }

        void UpdateRotation()
        {
            var mouseY       = Mouse.current.delta.y.ReadValue();
            var currentPitch = cameraRotation.x;
            var pitchChange  = -mouseY * mouseSensitivity;
            var newPitch     = Mathf.Clamp(currentPitch + pitchChange, -90, 90);
            var newYaw       = originalParent.transform.eulerAngles.y;

            cameraRotation        = new(newPitch, newYaw, 0);
            transform.eulerAngles = cameraRotation;
        }

        void Update()
        {
            UpdatePosition();
            UpdateRotation();
        }
    }
}