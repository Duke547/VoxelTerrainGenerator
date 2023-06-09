using UnityEngine;
using UnityEngine.InputSystem;
using VoxelWorld.Classes;

namespace VoxelWorld.Scripts
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        Vector3 cameraRotation;

        [Min(0)]
        public float speed = 4;

        [Min(0)]
        public float mouseSensitivity = 0.2f;

        private CharacterController characterController => GetComponent<CharacterController>();

        private new Camera camera => GetComponentInChildren<Camera>();

        private Vector3 movementInput
        {
            get
            {
                var keyboard  = Keyboard.current;
                var direction = new Vector3();

                if (keyboard.wKey.isPressed) direction += transform.forward;
                if (keyboard.sKey.isPressed) direction -= transform.forward;
                if (keyboard.aKey.isPressed) direction -= transform.right;
                if (keyboard.dKey.isPressed) direction += transform.right;

                return direction;
            }
        }

        public Vector3 velocity => speed * movementInput;

        public BlockTarget target
        {
            get
            {
                if (camera != null)
                    return BlockTarget.GetTarget(camera);

                return null;
            }
        }

        void UpdateMovement()
        {
            characterController.SimpleMove(velocity);
        }

        void UpdateRotation()
        {
            var mouseX = Mouse.current.delta.x.ReadValue();

            transform.Rotate(Vector3.up, mouseX * mouseSensitivity, Space.Self);
        }

        void UpdateCameraPitch()
        {
            if (camera != null)
            {
                var mouseY       = Mouse.current.delta.y.ReadValue();
                var currentPitch = cameraRotation.x;
                var amount       = -mouseY * mouseSensitivity;
                var newPitch     = Mathf.Clamp(currentPitch + amount, -90, 90);

                cameraRotation                    = new(newPitch, 0, 0);
                camera.transform.localEulerAngles = cameraRotation;
            }
        }

        void Update()
        {
            UpdateMovement();
            UpdateRotation();
            UpdateCameraPitch();
        }
    }
}