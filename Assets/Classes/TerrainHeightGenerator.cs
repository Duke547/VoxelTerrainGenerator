using UnityEngine;

public static class TerrainHeightGenerator
{
    public static float[,] Generate(int width, int length)
    {
        var data = new float[width, length];

        for (int y = 0; y < length; y++)
        {
            for (int x = 0; x < width; x++)
            {
                data[x, y] = Mathf.PerlinNoise(x / 100f, y / 100f);
            }
        }

        return data;
    }
}