using UnityEngine;
using static UnityEngine.Vector3;

namespace VoxelWorld
{
    public static class TerrainMeshGenerator
    {
        static void GenerateBlockFace(MeshCache mesh, Vector3Int blockCenter, Vector3 direction)
        {
            var up      = direction;
            var left    = zero;
            var forward = zero;

            OrthoNormalize(ref up, ref left, ref forward);

            var right    = -left;
            var backward = -forward;

            var distance   = 0.5f;
            var faceCenter = blockCenter + distance * up;

            var v1 = faceCenter + distance * forward + distance * left;
            var v2 = v1 + distance * 2 * right;
            var v3 = v2 + distance * 2 * backward;
            var v4 = v3 + distance * 2 * left;

            mesh.Vertices.Add(new() { Position = v1, Normal = up, UV = new(0, 0) });
            mesh.Vertices.Add(new() { Position = v2, Normal = up, UV = new(0, 1) });
            mesh.Vertices.Add(new() { Position = v3, Normal = up, UV = new(1, 1) });
            mesh.Vertices.Add(new() { Position = v4, Normal = up, UV = new(1, 0) });

            var vCount = mesh.Vertices.Count;

            mesh.Indices.Add(vCount - 4);
            mesh.Indices.Add(vCount - 3);
            mesh.Indices.Add(vCount - 2);
            mesh.Indices.Add(vCount - 2);
            mesh.Indices.Add(vCount - 1);
            mesh.Indices.Add(vCount - 4);
        }

        static void TryGenerateAdjacentFace(World world, MeshCache mesh, BlockType block, Vector3Int blockPosition, Vector3 direction)
        {
            var adjacentPosition = Vector3Int.RoundToInt(blockPosition + direction.normalized);
            var adjacentBlock    = world.GetBlock(adjacentPosition);

            if (adjacentBlock != null && block.IsSolid != adjacentBlock.IsSolid)
            {
                if (block.IsSolid)
                    GenerateBlockFace(mesh, blockPosition, direction);
                else
                    GenerateBlockFace(mesh, adjacentPosition, -direction);
            }
        }

        static void GenerateBlock(World world, MeshCache mesh, Vector3Int position)
        {
            var current = world.GetBlock(position);

            if (current == null)
                return;

            TryGenerateAdjacentFace(world, mesh, current, position, right  );
            TryGenerateAdjacentFace(world, mesh, current, position, up     );
            TryGenerateAdjacentFace(world, mesh, current, position, forward);
        }

        static void GenerateChunkBlocks(WorldChunk chunk, MeshCache mesh)
        {
            var area = chunk.Area;

            for (int z = area.y; z < area.height + area.y; z++)
            {
                for (int y = 0; y < chunk.World.Height; y++)
                {
                    for (int x = area.x; x < area.width + area.x; x++)
                        GenerateBlock(chunk.World, mesh, new(x, y, z));
                }
            }
        }

        public static MeshCache GenerateChunkMesh(WorldChunk chunk)
        {
            var mesh = new MeshCache();

            GenerateChunkBlocks(chunk, mesh);

            return mesh;
        }
    }
}