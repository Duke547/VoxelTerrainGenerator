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
        
        CharacterController characterController => GetComponent<CharacterController>();

        new Camera camera => GetComponentInChildren<Camera>();

        Vector3 movementInput
        {
            get
            {
                var keyboard  = Keyboard.current;
                var direction = Vector3.zero;

                if (keyboard.wKey.isPressed) direction += transform.forward;
                if (keyboard.sKey.isPressed) direction -= transform.forward;
                if (keyboard.aKey.isPressed) direction -= transform.right;
                if (keyboard.dKey.isPressed) direction += transform.right;

                return direction;
            }
        }

        public Vector3 velocity => speed * movementInput;

        public bool onGround => characterController.collisionFlags.HasFlag(CollisionFlags.CollidedBelow);

        public float heightAboveGround
        {
            get
            {
                GetCapsule(out var capsuleStart, out var capsuleEnd);

                Physics.CapsuleCast(capsuleStart, capsuleEnd, characterController.radius, Vector3.down, out var hit);

                return hit.distance;
            }
        }

        public BlockTarget target
        {
            get
            {
                if (camera != null)
                    return BlockTarget.GetTarget(camera);

                return null;
            }
        }

        void GetCapsule(out Vector3 start, out Vector3 end)
        {
            var offset = characterController.height / 2 - characterController.radius;

            start = transform.position + Vector3.down * offset;
            end   = transform.position + Vector3.up   * offset;
        }

        void StepDown()
            => characterController.Move(Vector3.down * characterController.stepOffset);

        void UpdateMovement()
        {
            var onGround = this.onGround;
            
            characterController.SimpleMove(velocity);

            if (onGround && heightAboveGround <= characterController.stepOffset)
                StepDown();
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