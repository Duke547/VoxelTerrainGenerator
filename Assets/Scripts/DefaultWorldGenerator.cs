using Unity.Profiling;
using UnityEngine;
using VoxelWorld.Classes;

namespace VoxelWorld.Scripts
{
    public class DefaultWorldGenerator : WorldGenerator
    {
        public int Size = 20;

        static float[,] GenerateSurfaceData(int size)
        {
            using (new ProfilerMarker("TerrainLoader.GenerateSurfaceData").Auto())
            {
                var data = new float[size, size];

                for (var z = 0; z < size; z++)
                {
                    for (var x = 0; x < size; x++)
                    {
                        data[x, z] = Mathf.PerlinNoise(x / 100f, z / 100f);
                    }
                }

                return data;
            }
        }

        protected override World Generate()
        {
            using (new ProfilerMarker("DefaultWorldGenerator.Generate").Auto())
            {
                var world = new World(Size);
                var surface = GenerateSurfaceData(Size);

                for (var z = 0; z < world.Length; z++)
                {
                    for (var x = 0; x < world.Width; x++)
                    {
                        for (var y = 0; y < world.Height; y++)
                        {
                            if (y < surface[x, z] * world.Height)
                                world.Blocks[x, y, z] = BlockID.Dirt;
                        }
                    }
                }

                world.PlayerSpawn = world.FindSurface(Size/2, Size/2) + Vector3.up * 3;

                return world;
            }
        }
    }
}