using System.Collections.Generic;
using UnityEngine;

public static class TerrainMeshGenerator
{
    static Mesh LoadMesh(BlockType[,,] terrain, Vector3Int location)
    {
        var block = terrain[location.x, location.y, location.z];

        return Resources.Load<Mesh>("Meshes/Blocks/" + block.MeshName);
    }

    static void GenerateBlock(BlockType[,,] terrain, Vector3Int location, List<CombineInstance> blocks)
    {
        var mesh = LoadMesh(terrain, location);

        var block = new CombineInstance
        {
            mesh      = mesh,
            transform = Matrix4x4.Translate(location)
        };

        blocks.Add(block);
    }

    static CombineInstance[] GenerateBlocks(BlockType[,,] terrain)
    {
        var blocks = new List<CombineInstance>();

        for (int z = 0; z < terrain.GetLength(2); z++)
        {
            for (int y = 0; y < terrain.GetLength(1); y++)
            {
                for (int x = 0; x < terrain.GetLength(0); x++)
                {
                    if (terrain[x, y, z].IsSolid)
                        GenerateBlock(terrain, new(x, y, z), blocks);
                }
            }
        }

        return blocks.ToArray();
    }

    public static Mesh GenerateMesh(BlockType[,,] terrain)
    {
        var mesh   = new Mesh();
        var blocks = GenerateBlocks(terrain);

        mesh.CombineMeshes(blocks, true, true);

        return mesh;
    }
}