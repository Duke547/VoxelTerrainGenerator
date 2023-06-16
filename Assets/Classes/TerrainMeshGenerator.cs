using UnityEngine;
using static UnityEngine.Vector3;

namespace VoxelWorld.Classes
{
    public static class TerrainMeshGenerator
    {
        static void GenerateBlockFace(MeshCache mesh, Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, Vector3 direction)
        {
            int i1 = mesh.Vertices.Count;
            int i2 = i1 + 1;
            int i3 = i2 + 1;
            int i4 = i3 + 1;

            mesh.Vertices.Add(i1, v1);
            mesh.Vertices.Add(i2, v2);
            mesh.Vertices.Add(i3, v3);
            mesh.Vertices.Add(i4, v4);

            mesh.Indices.Add(i1);
            mesh.Indices.Add(i2);
            mesh.Indices.Add(i3);
            mesh.Indices.Add(i3);
            mesh.Indices.Add(i4);
            mesh.Indices.Add(i1);

            mesh.Normals.Add(i1, direction);
            mesh.Normals.Add(i2, direction);
            mesh.Normals.Add(i3, direction);
            mesh.Normals.Add(i4, direction);

            mesh.UVs.Add(i1, new Vector2(0, 0));
            mesh.UVs.Add(i2, new Vector2(1, 0));
            mesh.UVs.Add(i3, new Vector2(1, 1));
            mesh.UVs.Add(i4, new Vector2(0, 1));
        }

        static void GetBlockFaceVertices(Vector3Int position, Vector3 axis, out Vector3 v1, out Vector3 v2, out Vector3 v3, out Vector3 v4)
        {
            if (axis == right)
            {
                v1 = position + right * 0.5f + down * 0.5f + forward * 0.5f;
                v2 = v1       + back;
                v3 = v2       + up;
                v4 = v3       + forward;
            }

            else if (axis == up)
            {
                v1 = position + up * 0.5f + left * 0.5f + forward * 0.5f;
                v2 = v1       + right;
                v3 = v2       + back;
                v4 = v3       + left;
            }

            else
            {
                v1 = position + forward * 0.5f + down * 0.5f + left * 0.5f;
                v2 = v1 + right;
                v3 = v2 + up;
                v4 = v3 + left;
            }
        }

        static void TryGenerateAdjacentFace(MeshCache mesh, BlockType first, BlockType second, Vector3Int position, Vector3 axis)
        {
            if (first.IsSolid != second.IsSolid)
            {
                GetBlockFaceVertices(position, axis, out var v1, out var v2, out var v3, out var v4);

                if (first.IsSolid)
                    GenerateBlockFace(mesh, v1, v2, v3, v4, axis);
                else
                    GenerateBlockFace(mesh, v4, v3, v2, v1, -axis);
            }
        }

        static void GenerateBlock(World world, MeshCache mesh, Vector3Int position)
        {
            var current = world.GetBlock(Vector3Int.RoundToInt(position));

            if (current == null)
                return;

            var rightAdjacent = world.GetBlock(Vector3Int.RoundToInt(position + right  ));
            var topAdjacent   = world.GetBlock(Vector3Int.RoundToInt(position + up     ));
            var frontAdjacent = world.GetBlock(Vector3Int.RoundToInt(position + forward));

            if (rightAdjacent != null)
                TryGenerateAdjacentFace(mesh, current, rightAdjacent, position, right);
            
            if (topAdjacent != null)
                TryGenerateAdjacentFace(mesh, current, topAdjacent,   position, up);

            if (frontAdjacent != null)
                TryGenerateAdjacentFace(mesh, current, frontAdjacent, position, forward);
        }

        static void GenerateChunkBlocks(World world, MeshCache mesh, RectInt rect)
        {
            for (int z = rect.y; z < rect.height + rect.y; z++)
            {
                for (int y = 0; y < world.Height; y++)
                {
                    for (int x = rect.x; x < rect.width + rect.x; x++)
                        GenerateBlock(world, mesh, new(x, y, z));
                }
            }
        }

        public static MeshCache GenerateChunkMesh(World world, RectInt rect)
        {
            var mesh = new MeshCache();

            GenerateChunkBlocks(world, mesh, rect);

            return mesh;
        }
    }
}