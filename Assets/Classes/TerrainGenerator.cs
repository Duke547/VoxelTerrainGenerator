public static class TerrainGenerator
{
    public static void Generate(World world)
    {
        for (int z = 0; z < world.Length; z++)
        {
            for (int y = 0; y < world.Height; y++)
            {
                for (int x = 0; x < world.Width; x++)
                {
                    if (y <= world.Height / 2)
                        world.Terrain[x, y, z] = BlockTypes.Dirt;
                }
            }
        }
    }
}