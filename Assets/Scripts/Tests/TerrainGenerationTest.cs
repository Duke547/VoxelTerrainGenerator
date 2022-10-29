using UnityEngine;

public class TerrainGenerationTest : MonoBehaviour
{
    public int Width  = 20;
    public int Length = 20;
    public int Height = 20;

    public BlockType bl;

    void Start()
    {
        var world = new World(Width, Height, Length);

        TerrainGenerator.Generate(world);
        TerrainMeshGenerator.Generate(world);
    }
}
