using UnityEngine;

public static class TestWorldTerrain
{
    public static BlockType[,,] Generate()
    {
        var blocks = new BlockType[3,3,3];

        for (int z = 0; z < blocks.GetLength(2); z++)
        {
            for (int y = 0; y < blocks.GetLength(1); y++)
            {
                for (int x = 0; x < blocks.GetLength(0); x++)
                {
                    blocks[x, y, z] = BlockTypes.Air;
                }
            }
        }

        blocks[0, 0, 0] = BlockTypes.Dirt;
        blocks[1, 0, 0] = BlockTypes.Dirt;
        blocks[2, 0, 0] = BlockTypes.Dirt;
        blocks[0, 0, 1] = BlockTypes.Dirt;
        blocks[1, 0, 1] = BlockTypes.Dirt;
        blocks[2, 0, 1] = BlockTypes.Dirt;
        blocks[0, 0, 2] = BlockTypes.Dirt;
        blocks[1, 0, 2] = BlockTypes.Dirt;
        blocks[2, 0, 2] = BlockTypes.Dirt;
        blocks[1, 1, 1] = BlockTypes.Dirt;

        return blocks;
    }
}