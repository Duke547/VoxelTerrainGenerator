using System.Collections.Generic;
using UnityEngine;

public static class TerrainMeshGenerator
{
    static CombineInstance GenerateBlockFace(Vector3[] vertices, Vector3 origin, Vector3 direction)
    {
        CombineInstance face = new CombineInstance();
        Mesh            mesh = new Mesh();

        mesh.SetVertices(new[] { vertices[0], vertices[1], vertices[2], vertices[3] });
        mesh.SetIndices (new[] { 0, 1, 2, 2, 3, 0 }, MeshTopology.Triangles, 0);
        mesh.SetNormals (new[] { direction, direction, direction, direction } );

        face.mesh      = mesh;
        face.transform = Matrix4x4.Translate(origin);

        return face;
    }

    static CombineInstance[] GenerateBlock(Vector3 location)
    {
        List<CombineInstance> faces = new List<CombineInstance>();

        var v1 = new Vector3(-0.5f, -0.5f, -0.5f);
        var v2 = new Vector3( 0.5f, -0.5f, -0.5f);
        var v3 = new Vector3( 0.5f, -0.5f,  0.5f);
        var v4 = new Vector3(-0.5f, -0.5f,  0.5f);
        var v5 = new Vector3(-0.5f,  0.5f, -0.5f);
        var v6 = new Vector3( 0.5f,  0.5f, -0.5f);
        var v7 = new Vector3( 0.5f,  0.5f,  0.5f);
        var v8 = new Vector3(-0.5f,  0.5f,  0.5f);

        faces.Add(GenerateBlockFace(new[] { v8, v7, v6, v5 }, location, Vector3.up));
        faces.Add(GenerateBlockFace(new[] { v1, v2, v3, v4 }, location, Vector3.down));
        faces.Add(GenerateBlockFace(new[] { v1, v4, v8, v5 }, location, Vector3.left));
        faces.Add(GenerateBlockFace(new[] { v6, v7, v3, v2 }, location, Vector3.right));
        faces.Add(GenerateBlockFace(new[] { v7, v8, v4, v3 }, location, Vector3.forward));
        faces.Add(GenerateBlockFace(new[] { v5, v6, v2, v1 }, location, Vector3.back));

        return faces.ToArray();
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
                        blocks.AddRange(GenerateBlock(new(x, y, z)));
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