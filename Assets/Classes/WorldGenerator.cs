using UnityEngine;

namespace VoxelWorld.Classes
{
    public static class WorldGenerator
    {
        static float[,] GenerateSurfaceData(int size)
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

        public static World Generate(int size)
        {
            var world   = new World(size);
            var surface = GenerateSurfaceData(size);

            for (var z = 0; z < world.Length; z++)
            {
                for (var x = 0; x < world.Width; x++)
                {
                    for (var y = 0; y < world.Height; y++)
                    {
                        if (y < surface[x, z] * world.Height)
                            world.SetBlock(new(x, y, z));
                    }
                }
            }

            world.PlayerSpawn = world.FindSurface(size/2, size/2) + Vector3.up * 3;

            return world;
        }
    }
}