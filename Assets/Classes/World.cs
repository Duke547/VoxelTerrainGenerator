using Unity.Profiling;
using UnityEngine;

namespace VoxelWorld.Classes
{
    public class World
    {
        public BlockID[] Blocks { get; }

        public int Width { get; private set; }

        public int Length { get; private set; }

        public int Height { get; private set; }

        public Vector3 PlayerSpawn { get; set; }

        int GetFlattenedBlockIndex(int x, int y, int z)
            => y * Width * Length + z * Width + x;

        public BlockID this[int x, int y, int z]
        {
            get => Blocks[GetFlattenedBlockIndex(x, y, z)];
            set => Blocks[GetFlattenedBlockIndex(x, y, z)] = value;
        }

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

                var blockID = this[location.x, location.y, location.z];

                return BlockType.GetType(blockID);
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

        public World(int size)
        {
            Width  = size;
            Length = size;
            Height = 100;
            Blocks = new BlockID[Width * Length * Height];
        }

        public World() : this(0) { }
    }
}