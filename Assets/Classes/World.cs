using Unity.Profiling;
using UnityEngine;

namespace VoxelWorld.Classes
{
    public class World
    {
        public BlockID[,,] Blocks { get; }

        public BlockID this[int x, int y, int z]
        {
            get => Blocks[x, y, z];
            set => Blocks[x, y, z] = value;
        }

        public int Width => Blocks.GetLength(0);

        public int Length => Blocks.GetLength(2);

        public int Height => Blocks.GetLength(1);

        public Vector3 PlayerSpawn { get; set; }

        public BlockType GetBlock(Vector3Int location)
        {
            using (new ProfilerMarker("World.GetBlock").Auto())
            {
                if (location.x < 0 || location.x >= Width)
                    return null;

                if (location.y < 0 || location.y >= Height)
                    return null;

                if (location.z < 0 || location.z >= Length)
                    return null;

                return BlockType.GetType(Blocks[location.x, location.y, location.z]);
            }
        }

        public BlockType GetBlock(int x, int y, int z)
            => GetBlock(new(x, y, z));

        public Vector3 FindSurface(int x, int z)
        {
            using (new ProfilerMarker("World.FindSurface").Auto())
            {
                var last = new Vector3Int(x, Height - 1, z);

                for (int y = last.y; y >= 0; y--)
                {
                    if (!GetBlock(x, y, z).IsSolid)
                        last = new(x, y, z);
                    else
                        break;
                }

                return last;
            }
        }

        public BlockType FindSurfaceBlock(int x, int y)
            => GetBlock(Vector3Int.RoundToInt(FindSurface(x, y)));

        public World(int size) =>
            Blocks = new BlockID[size, 100, size];

        public World(BlockID[,,] blocks) =>
            Blocks = blocks;

        public World() : this(0) { }
    }
}