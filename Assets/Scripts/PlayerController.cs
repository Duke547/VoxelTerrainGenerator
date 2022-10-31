using UnityEngine;
using UnityEngine.InputSystem;

namespace VoxelWorld.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        Vector3 _cameraRotation;

        public float maxSpeed          = 4;
        public float cameraSensitivity = 0.2f;

        void UpdateMovement()
        {
            if (TryGetComponent(out CharacterController controller))
            {
                var keyboard  = Keyboard.current;
                var direction = new Vector3();

                if (keyboard.wKey.isPressed) direction += transform.forward;
                if (keyboard.sKey.isPressed) direction -= transform.forward;
                if (keyboard.aKey.isPressed) direction -= transform.right;
                if (keyboard.dKey.isPressed) direction += transform.right;

                controller.SimpleMove(maxSpeed * direction);
            }
        }

        void UpdateRotation()
        {
            var mouse  = Mouse.current;
            var mouseX = mouse.delta.x.ReadValue();
            var mouseY = mouse.delta.y.ReadValue();
            var camera = GetComponentInChildren<Camera>();

            transform.Rotate(Vector3.up, mouseX * cameraSensitivity, Space.Self);

            if (camera != null)
            {
                _cameraRotation += new Vector3(-mouseY * cameraSensitivity, 0, 0);

                if (_cameraRotation.x > 90)
                    _cameraRotation.x = 90;

                else if (_cameraRotation.x < -90)
                    _cameraRotation.x = -90;

                camera.transform.localEulerAngles = _cameraRotation;
            }
        }

        void Update()
        {
            UpdateRotation();
            UpdateMovement();
        }
    }
}