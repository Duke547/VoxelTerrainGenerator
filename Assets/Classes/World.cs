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

        public BlockType GetBlock(Vector3Int position)
        {
            if (position.x < 0 || position.x >= Width)
                return null;

            if (position.y < 0 || position.y >= Height)
                return null;

            if (position.z < 0 || position.z >= Length)
                return null;

            var blockID = this[position.x, position.y, position.z];

            return BlockType.GetType(blockID);
        }

        public BlockType GetBlock(int x, int y, int z)
            => GetBlock(new(x, y, z));

        public void SetBlock(Vector3Int position)
            => this[position.x, position.y, position.z] = BlockID.Dirt;

        public void RemoveBlock(Vector3Int position)
            => this[position.x, position.y, position.z] = BlockID.Air;

        public Vector3 FindSurface(int x, int z)
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