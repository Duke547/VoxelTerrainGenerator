using UnityEngine.Rendering;
using UnityEngine;

using static System.Math;
using Unity.Profiling;

namespace VoxelWorld.Classes
{
    public static class TerrainGenerator
    {
        public static GameObject GenerateChunk(World world, RectInt rect)
        {
            using (new ProfilerMarker($"{nameof(TerrainGenerator)}.GenerateChunk").Auto())
            {
                rect.x      = Max(rect.x, 0);
                rect.y      = Max(rect.y, 0);
                rect.width  = Min(rect.width,  world.Width  - rect.x);
                rect.height = Min(rect.height, world.Length - rect.y);

                var chunk        = new GameObject("Terrain Chunk") { tag = "Terrain Chunk" };
                var meshFilter   = chunk.AddComponent<MeshFilter>();
                var meshRenderer = chunk.AddComponent<MeshRenderer>();
                var collider     = chunk.AddComponent<MeshCollider>();
                var mesh         = TerrainMeshGenerator.GenerateChunkMesh(world, rect);

                meshRenderer.shadowCastingMode = ShadowCastingMode.TwoSided;
                meshRenderer.material          = Resources.Load<Material>("Materials/Terrain");

                meshFilter.mesh     = mesh;
                collider.sharedMesh = mesh;

                return chunk;
            }
        }
    }
}