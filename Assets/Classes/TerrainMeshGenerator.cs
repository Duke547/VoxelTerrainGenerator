using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

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

    static CombineInstance[] GenerateBlock(World world, Vector3Int location)
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

        var topAdjacent    = world.GetBlock(location + Vector3Int.up);
        var bottomAdjacent = world.GetBlock(location + Vector3Int.down);
        var leftAdjacent   = world.GetBlock(location + Vector3Int.left);
        var rightAdjacent  = world.GetBlock(location + Vector3Int.right);
        var frontAdjacent  = world.GetBlock(location + Vector3Int.forward);
        var backAdjacent   = world.GetBlock(location + Vector3Int.back);

        if (topAdjacent != null && !topAdjacent.IsSolid)
            faces.Add(GenerateBlockFace(new[] { v8, v7, v6, v5 }, location, Vector3.up));

        if (bottomAdjacent != null && !bottomAdjacent.IsSolid)
            faces.Add(GenerateBlockFace(new[] { v1, v2, v3, v4 }, location, Vector3.down));

        if (leftAdjacent != null && !leftAdjacent.IsSolid)
            faces.Add(GenerateBlockFace(new[] { v1, v4, v8, v5 }, location, Vector3.left));

        if (rightAdjacent != null && !rightAdjacent.IsSolid)
            faces.Add(GenerateBlockFace(new[] { v6, v7, v3, v2 }, location, Vector3.right));

        if (frontAdjacent != null && !frontAdjacent.IsSolid)
            faces.Add(GenerateBlockFace(new[] { v7, v8, v4, v3 }, location, Vector3.forward));

        if (backAdjacent != null && !backAdjacent.IsSolid)
            faces.Add(GenerateBlockFace(new[] { v5, v6, v2, v1 }, location, Vector3.back));

        return faces.ToArray();
    }

    static CombineInstance[] GenerateBlocks(World world)
    {
        var terrainData = world.Terrain;
        var blocks = new List<CombineInstance>();

        for (int z = 0; z < terrainData.GetLength(2); z++)
        {
            for (int y = 0; y < terrainData.GetLength(1); y++)
            {
                for (int x = 0; x < terrainData.GetLength(0); x++)
                {
                    if (terrainData[x, y, z].IsSolid)
                        blocks.AddRange(GenerateBlock(world, new(x, y, z)));
                }
            }
        }

        return blocks.ToArray();
    }

    static Mesh GenerateMesh(World world)
    {
        var mesh   = new Mesh() { indexFormat = IndexFormat.UInt32 };
        var blocks = GenerateBlocks(world);

        mesh.CombineMeshes(blocks, true, true);

        return mesh;
    }

    public static GameObject Generate(World world)
    {
        var terrainObject = new GameObject("Terrain");
        var meshFilter    = terrainObject.AddComponent<MeshFilter>();
        var meshRenderer  = terrainObject.AddComponent<MeshRenderer>();

        meshRenderer.materials[0] = Resources.Load<Material>("Materials/Default");

        meshFilter.mesh = GenerateMesh(world);

        return terrainObject;
    }
}