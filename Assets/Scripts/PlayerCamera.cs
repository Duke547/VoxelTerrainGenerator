using UnityEngine;
using UnityEngine.InputSystem;

namespace VoxelWorld.Scripts
{
    [RequireComponent(typeof(Camera))]
    public class PlayerCamera : MonoBehaviour
    {
        Vector3 cameraRotation;

        [Min(0)]
        public float mouseSensitivity = 0.2f;
        
        new Camera camera => GetComponent<Camera>();

        void Update()
        {
            if (camera != null)
            {
                var mouseY       = Mouse.current.delta.y.ReadValue();
                var currentPitch = cameraRotation.x;
                var amount       = -mouseY * mouseSensitivity;
                var newPitch     = Mathf.Clamp(currentPitch + amount, -90, 90);

                cameraRotation = new(newPitch, 0, 0);
                camera.transform.localEulerAngles = cameraRotation;
            }
        }
    }
}