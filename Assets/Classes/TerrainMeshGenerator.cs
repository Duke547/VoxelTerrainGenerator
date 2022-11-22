using Unity.Profiling;
using UnityEngine;

using static UnityEngine.Color;
using static UnityEngine.Vector3;

using UnityMesh = UnityEngine.Mesh;

namespace VoxelWorld.Classes
{
    public static class TerrainMeshGenerator
    {
        static void GenerateBlockFace(Mesh mesh, Vertex v1, Vertex v2, Vertex v3, Vertex v4, Vector3 direction)
        {
            using (new ProfilerMarker($"{nameof(TerrainMeshGenerator)}.GenerateBlockFace").Auto())
            {
                int i1 = mesh.Vertices.Count;
                int i2 = i1 + 1;
                int i3 = i2 + 1;
                int i4 = i3 + 1;

                mesh.Vertices.AddRange(new[] { v1, v2, v3, v4 });
                mesh.Indices .AddRange(new[] { i1, i2, i3, i3, i4, i1 });
                mesh.Normals .AddRange(new[] { direction, direction, direction, direction });
                mesh.UVs     .AddRange(new[] { new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1) });
            }
        }

        static bool TryGenerateBlockFace(World world, Mesh mesh, Vertex v1, Vertex v2, Vertex v3, Vertex v4, Vector3Int origin, Vector3 direction)
        {
            using (new ProfilerMarker($"{nameof(TerrainMeshGenerator)}.TryGenerateBlockFace").Auto())
            {
                var adjacent = world.GetBlock(Vector3Int.RoundToInt(origin + direction));
                
                if (adjacent != null && !adjacent.IsSolid)
                {
                    GenerateBlockFace(mesh, v1, v2, v3, v4, direction);

                    return true;
                }

                return false;
            }
        }

        static void GenerateBlock(World world, Mesh mesh, Vector3Int location)
        {
            using (new ProfilerMarker($"{nameof(TerrainMeshGenerator)}.GenerateBlock").Auto())
            {
                var vRightBackBottom  = new Vertex(location);
                var vLeftBackBottom   = new Vertex(vRightBackBottom  + left);
                var vLeftFrontBottom  = new Vertex(vLeftBackBottom   + forward);
                var vRightFrontBottom = new Vertex(vLeftFrontBottom  + right);
                var vRightFrontTop    = new Vertex(vRightFrontBottom + up);
                var vLeftFrontTop     = new Vertex(vRightFrontTop    + left);
                var vLeftBackTop      = new Vertex(vLeftFrontTop     + back);
                var vRightBackTop     = new Vertex(vLeftBackTop      + right);

                TryGenerateBlockFace(world, mesh, vRightBackBottom, vRightFrontBottom, vLeftFrontBottom,  vLeftBackBottom,   location, down   );
                TryGenerateBlockFace(world, mesh, vRightBackTop,    vLeftBackTop,      vLeftFrontTop,     vRightFrontTop,    location, up     );
                TryGenerateBlockFace(world, mesh, vRightFrontTop,   vLeftFrontTop,     vLeftFrontBottom,  vRightFrontBottom, location, forward);
                TryGenerateBlockFace(world, mesh, vRightBackTop,    vRightBackBottom,  vLeftBackBottom,   vLeftBackTop,      location, back   );
                TryGenerateBlockFace(world, mesh, vLeftFrontTop,    vLeftBackTop,      vLeftBackBottom,   vLeftFrontBottom,  location, left   );
                TryGenerateBlockFace(world, mesh, vRightBackTop,    vRightFrontTop,    vRightFrontBottom, vRightBackBottom,  location, right  );
            }
        }

        static void GenerateChunkBlocks(World world, Mesh mesh, RectInt rect)
        {
            using (new ProfilerMarker($"{nameof(TerrainMeshGenerator)}.GenerateChunkBlocks").Auto())
            {
                for (int z = rect.y; z < rect.height + rect.y; z++)
                {
                    for (int y = 0; y < world.Height; y++)
                    {
                        for (int x = rect.x; x < rect.width + rect.x; x++)
                        {
                            if (BlockType.GetType(world[x, y, z]).IsSolid)
                                GenerateBlock(world, mesh, new(x, y, z));
                        }
                    }
                }
            }
        }

        public static UnityMesh GenerateChunkMesh(World world, RectInt rect)
        {
            using (new ProfilerMarker($"{nameof(TerrainMeshGenerator)}.GenerateChunkMesh").Auto())
            {
                var mesh = new Mesh();

                GenerateChunkBlocks(world, mesh, rect);

                return mesh.ToUnityMesh();
            }
        }
    }
}