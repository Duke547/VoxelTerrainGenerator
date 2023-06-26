using UnityEngine;
using VoxelWorld.Scripts;

namespace VoxelWorld.Classes
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