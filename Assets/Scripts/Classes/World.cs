﻿using UnityEngine;

namespace VoxelWorld
{
    public class World
    {
        byte[,,] Blocks { get; }

        public int Width => Blocks.GetLength(0);

        public int Length => Blocks.GetLength(2);

        public int Height => Blocks.GetLength(1);

        public Vector3 PlayerSpawn { get; set; }

        public BlockType GetBlock(Vector3Int position)
        {
            //using (new ProfilerMarker($"{nameof(World)}.{nameof(GetBlock)}").Auto())
            //{
            
            if (position.x < 0 || position.x >= Width)
                return null;

            if (position.y < 0 || position.y >= Height)
                return null;

            if (position.z < 0 || position.z >= Length)
                return null;

            var blockID = Blocks[position.x, position.y, position.z];

            return BlockType.Types[blockID]; 
            
            //}
        }

        public BlockType GetBlock(int x, int y, int z)
            => GetBlock(new(x, y, z));

        public void SetBlock(Vector3Int position, string blockTypeName)
            => Blocks[position.x, position.y, position.z] = (byte)BlockType.GetBlockTypeID(blockTypeName);

        public Vector3 FindSurface(int x, int z)
        {
            //using (new ProfilerMarker($"{nameof(World)}.{nameof(FindSurface)}").Auto())
            //{
                var last = new Vector3Int(x, Height - 1, z);

                for (int y = last.y; y >= 0; y--)
                {
                    if (!GetBlock(x, y, z).IsSolid)
                        last = new(x, y, z);
                    else
                        break;
                }

                return last; 
            //}
        }

        public BlockType FindSurfaceBlock(int x, int y)
            => GetBlock(Vector3Int.RoundToInt(FindSurface(x, y)));

        public World(byte[,,] blocks)
            => Blocks = blocks;
    }
}