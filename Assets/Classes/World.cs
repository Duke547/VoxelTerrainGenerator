using UnityEngine;

public class World
{
    public BlockType[,,] Terrain { get; }

    public int Width => Terrain.GetLength(0);

    public int Length => Terrain.GetLength(2);

    public int Height => Terrain.GetLength(1);

    public BlockType GetBlock(Vector3Int location)
    {
        if (location.x < 0 || location.x >= Width)
            return null;

        if (location.y < 0 || location.y >= Height)
            return null;

        if (location.z < 0 || location.z >= Length)
            return null;

        return Terrain[location.x, location.y, location.z];
    }

    void InitializeTerrain()
    {
        for (int z = 0; z < Length; z++)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Terrain[x, y, z] = BlockTypes.Air;
                }
            }
        }
    }

    public World(int width, int length, int height)
    {
        Terrain = new BlockType[width, height, length];

        InitializeTerrain();
    }
}