using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

namespace VoxelWorld.Scripts
{
    [ExcludeFromCoverage]
    [RequireComponent(typeof(PlayerController))]
    public class PlayerInput : MonoBehaviour
    {
        [Min(0.01f)]
        public float mouseSensitivity = 0.2f;

        public bool mouseControlEnabled { get; set; } = true;

        void ApplyInput()
        {
            var controller = GetComponent<PlayerController>();
            var camera     = PlayerCamera.current;
            var keyboard   = Keyboard.current;
            var mouse      = Mouse   .current;
            var mouseX     = mouse.delta.x.ReadValue();
            var mouseY     = mouse.delta.y.ReadValue();

            controller.movementInput = Vector3.zero;

            if (keyboard.wKey.isPressed) controller.movementInput += transform.forward;
            if (keyboard.sKey.isPressed) controller.movementInput -= transform.forward;
            if (keyboard.aKey.isPressed) controller.movementInput -= transform.right;
            if (keyboard.dKey.isPressed) controller.movementInput += transform.right;

            if (mouseControlEnabled)
            {
                controller.Look(mouseX * mouseSensitivity);

                if (camera != null)
                    camera.Look(mouseY * mouseSensitivity);
            }

            if (keyboard.spaceKey.wasPressedThisFrame)
                controller.Jump();

            if (mouse.leftButton.wasPressedThisFrame)
                controller.Strike();

            if (mouse.rightButton.wasPressedThisFrame)
                controller.Place();

            if (keyboard.escapeKey.wasPressedThisFrame)
                mouseControlEnabled = !mouseControlEnabled;
        }

        void CenterMouse()
        {
            if (PlayerCamera.current != null)
            {
                var camera = PlayerCamera.current.GetComponent<Camera>();
                Mouse.current.WarpCursorPosition(camera.pixelRect.center);
            }
        }

        void Update()
        {
            ApplyInput();

            if (mouseControlEnabled && Application.isFocused)
                CenterMouse();
        }
    }
}