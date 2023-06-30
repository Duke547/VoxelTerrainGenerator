//using Unity.Profiling;
using UnityEngine;

namespace VoxelWorld
{
    public static class WorldGenerator
    {
        public static float[,] GenerateSurfaceData(int size)
        {
            //using (new ProfilerMarker($"{nameof(WorldGenerator)}.{nameof(GenerateSurfaceData)}").Auto())
            //{
                var data = new float[size, size];

                for (var z = 0; z < size; z++)
                {
                    for (var x = 0; x < size; x++)
                    {
                        data[x, z] = Mathf.PerlinNoise(x / 100f, z / 100f);
                    }
                }

                return data; 
            //}
        }

        public static World Generate(int size)
        {
            var height  = 100;
            var blocks  = new byte[size, height, size];
            var surface = GenerateSurfaceData(size);

            for (var z = 0; z < size; z++)
            {
                for (var x = 0; x < size; x++)
                {
                    for (var y = 0; y < height; y++)
                    {
                        if (y < surface[x, z] * height)
                            blocks[x, y, z] = 1;
                    }
                }
            }

            var world = new World(blocks);

            world.PlayerSpawn = world.FindSurface(size/2, size/2) + Vector3.up * 3;

            return world;
        }
    }
}