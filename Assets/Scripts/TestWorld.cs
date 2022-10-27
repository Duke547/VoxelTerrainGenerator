using UnityEngine;

public class TestWorld : MonoBehaviour
{
    Mesh GenerateTerrainMesh()
    {
        var terrain = TestWorldTerrain.Generate();
        
        return TerrainMeshGenerator.GenerateMesh(terrain);
    }

    void GenerateTerrain()
    {
        var gameObject   = new GameObject("Terrain");
        var meshFilter   = gameObject.AddComponent<MeshFilter>();
        var meshRenderer = gameObject.AddComponent<MeshRenderer>();

        meshRenderer.materials[0] = Resources.Load<Material>("Materials/Default");

        meshFilter.mesh = GenerateTerrainMesh();
    }

    public void Start()
    {
        GenerateTerrain();
    }
}