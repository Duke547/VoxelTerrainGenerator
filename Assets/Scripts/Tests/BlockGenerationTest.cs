using UnityEngine;

public class BlockGenerationTest : MonoBehaviour
{
    void GenerateTerrainData(BlockType[,,] terrain)
    {
        terrain[0, 0, 0] = BlockTypes.Dirt;
        terrain[1, 0, 0] = BlockTypes.Dirt;
        terrain[2, 0, 0] = BlockTypes.Dirt;
        terrain[0, 0, 1] = BlockTypes.Dirt;
        terrain[1, 0, 1] = BlockTypes.Dirt;
        terrain[2, 0, 1] = BlockTypes.Dirt;
        terrain[0, 0, 2] = BlockTypes.Dirt;
        terrain[1, 0, 2] = BlockTypes.Dirt;
        terrain[2, 0, 2] = BlockTypes.Dirt;
        terrain[1, 1, 1] = BlockTypes.Dirt;
    }

    public void Start()
    {
        var world = new World(3, 3, 3);

        GenerateTerrainData(world.Terrain);
        
        TerrainMeshGenerator.Generate(world);
    }
}