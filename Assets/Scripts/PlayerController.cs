using UnityEngine;
using UnityEngine.InputSystem;

namespace VoxelWorld.Scripts
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Min(0)]
        public float speed = 4;

        [Min(0)]
        public float jumpStrength = 22;

        [Min(0)]
        public float jumpFallOff = 4;

        [Min(0)]
        public float mouseSensitivity = 0.2f;

        CharacterController characterController => GetComponent<CharacterController>();

        public bool mouseControlEnabled { get; set; } = true;

        Vector3 movementInput { get; set; }

        float jumpForce { get; set; }

        public bool onGround => characterController.collisionFlags.HasFlag(CollisionFlags.CollidedBelow);

        public Vector3 velocity { get; private set; }

        void ApplyInput()
        {
            var keyboard = Keyboard.current;
            var mouse    = Mouse   .current;

            movementInput = Vector3.zero;

            if (keyboard.wKey.isPressed) movementInput += transform.forward;
            if (keyboard.sKey.isPressed) movementInput -= transform.forward;
            if (keyboard.aKey.isPressed) movementInput -= transform.right;
            if (keyboard.dKey.isPressed) movementInput += transform.right;

            if (keyboard.spaceKey.wasPressedThisFrame)
                Jump();

            if (mouse.leftButton.wasPressedThisFrame)
                Strike();

            if (mouse.rightButton.wasPressedThisFrame)
                Place();

            if (keyboard.escapeKey.wasPressedThisFrame)
                mouseControlEnabled = !mouseControlEnabled;
        }

        void Jump()
        {
            if (onGround)
                jumpForce = jumpStrength;
        }

        void Strike()
        {
            if (PlayerCamera.current != null)
            {
                var targetBlock = PlayerCamera.current.TargetBlock;

                if (targetBlock != null)
                    targetBlock.terrainChunk.BreakBlock(targetBlock.position);
            }
        }

        void Place()
        {
            if (PlayerCamera.current != null)
            {
                var target = PlayerCamera.current.TargetEmptyPosition;

                if (target != null)
                    target.terrainChunk.PlaceBlock(target.position);
            }
        }

        void CenterMouse()
        {
            if (PlayerCamera.current != null)
            {
                var camera = PlayerCamera.current.GetComponent<Camera>();
                Mouse.current.WarpCursorPosition(camera.pixelRect.center);
            }
        }

        void UpdateMovement()
        {
            var verticalVelocity = Vector3.up * jumpForce + Physics.gravity;
            var cardinalVelocity = movementInput * speed;

            if (!onGround)
                cardinalVelocity = new(velocity.x, 0, velocity.z);

            velocity = verticalVelocity + cardinalVelocity;

            characterController.Move(velocity * Time.deltaTime);

            jumpForce = Mathf.Lerp(jumpForce, 0, jumpFallOff * Time.deltaTime);
        }

        void UpdateRotation()
        {
            var mouseX = Mouse.current.delta.x.ReadValue();

            transform.Rotate(Vector3.up, mouseX * mouseSensitivity, Space.Self);
        }

        void Update()
        {
            ApplyInput();
            UpdateMovement();

            if (mouseControlEnabled && Application.isFocused)
            {
                UpdateRotation();
                CenterMouse();
            }

            if (PlayerCamera.current != null)
                PlayerCamera.current.mouseControlEnabled = mouseControlEnabled;
        }
    }
}