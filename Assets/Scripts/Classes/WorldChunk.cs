using UnityEngine;

namespace VoxelWorld.Classes
{
    public record WorldChunk
    {
        public World world { get; private set; }

        public Vector2Int index { get; private set; }

        public int size { get; private set; }

        public Vector2Int position => index * size;

        public int height => world.Height;

        public RectInt area => new(position, new(size, size));

        public WorldChunk(World world, Vector2Int index, int size)
        {
            this.world  = world;
            this.index  = index;
            this.size   = size;
        }
    }
}