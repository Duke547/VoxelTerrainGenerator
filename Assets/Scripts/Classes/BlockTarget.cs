using UnityEngine;
using VoxelWorld.MonoBehaviors;

namespace VoxelWorld
{
    public class BlockTarget
    {
        public Vector3Int Position { get; }

        public TerrainChunk TerrainChunk { get; }

        public BlockTarget(Vector3Int position, TerrainChunk chunk)
        {
            Position     = position;
            TerrainChunk = chunk;
        }
    }
}