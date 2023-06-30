using UnityEngine;

namespace VoxelWorld
{
    public record WorldChunk
    {
        public World World { get; private set; }

        public Vector2Int Index { get; private set; }

        public int Size { get; private set; }

        public Vector2Int Position => Index * Size;

        public int Height => World.Height;

        public RectInt Area => new(Position, new(Size, Size));

        public WorldChunk(World world, Vector2Int index, int size)
        {
            this.World  = world;
            this.Index  = index;
            this.Size   = size;
        }
    }
}