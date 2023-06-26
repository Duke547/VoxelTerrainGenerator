using UnityEngine;
using VoxelWorld.Scripts;

namespace VoxelWorld.Classes
{
    public class BlockTarget
    {
        public Vector3Int Position { get; private set; }

        public TerrainChunk TerrainChunk { get; private set; }

        public BlockTarget(Vector3Int position, TerrainChunk chunk)
        {
            Position     = position;
            TerrainChunk = chunk;
        }
    }
}