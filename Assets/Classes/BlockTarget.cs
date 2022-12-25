using UnityEngine;
using VoxelWorld.Scripts;

namespace VoxelWorld.Classes
{
    public class BlockTarget
    {
        public Vector3Int position { get; private set; }

        public TerrainChunk chunk { get; private set; }

        public BlockTarget(Vector3Int position, TerrainChunk chunk)
        {
            this.position = position;
            this.chunk    = chunk;
        }

        public static BlockTarget GetTarget(Camera camera)
        {
            var cameraTransform = camera.transform;

            Physics.Raycast(cameraTransform.position, cameraTransform.forward, out var hit, 4);

            if (hit.collider != null)
            {
                var chunk = hit.collider.GetComponent<TerrainChunk>();

                if (chunk != null)
                {
                    var position = Vector3Int.RoundToInt(hit.point - hit.normal * 0.5f);

                    return new(position, chunk);
                }
            }

            return null;
        }
    }
}