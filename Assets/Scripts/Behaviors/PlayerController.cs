using UnityEngine;

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

        CharacterController characterController => GetComponent<CharacterController>();

        float jumpForce { get; set; }

        public bool onGround => characterController.collisionFlags.HasFlag(CollisionFlags.CollidedBelow);

        public Vector3 movementInput { get; set; }

        public Vector3 velocity { get; private set; }

        public void Look(float mouseX)
            => transform.Rotate(Vector3.up, mouseX, Space.Self);

        public void Jump()
        {
            if (onGround)
                jumpForce = jumpStrength;
        }

        public void Strike()
        {
            if (PlayerCamera.current != null)
            {
                var targetBlock = PlayerCamera.current.TargetBlock;

                if (targetBlock != null)
                    targetBlock.TerrainChunk.BreakBlock(targetBlock.Position);
            }
        }

        public void Place()
        {
            if (PlayerCamera.current != null)
            {
                var target = PlayerCamera.current.TargetEmptyPosition;

                if (target != null)
                    target.TerrainChunk.PlaceBlock(target.Position);
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

        void Update()
        {
            UpdateMovement();
        }
    }
}