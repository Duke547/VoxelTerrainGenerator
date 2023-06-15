using UnityEngine;
using VoxelWorld.Scripts;

namespace VoxelWorld.Classes
{
    public class BlockTarget
    {
        public Vector3Int position { get; private set; }

        public TerrainChunk terrainChunk { get; private set; }

        public BlockTarget(Vector3Int position, TerrainChunk chunk)
        {
            this.position     = position;
            this.terrainChunk = chunk;
        }
    }
}