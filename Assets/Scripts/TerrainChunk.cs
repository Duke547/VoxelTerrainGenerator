using System.Threading.Tasks;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Rendering;
using VoxelWorld.Classes;

using static System.Math;

namespace VoxelWorld.Scripts
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
    public class TerrainChunk : MonoBehaviour
    {
        public World world { get; internal set; }

        public RectInt rect { get; internal set; }

        public Vector2Int chunkIndex { get; internal set; }

        public bool loaded { get; private set; }

        async Task GenerateMesh()
        {
            var x      = Max(rect.x, 0);
            var y      = Max(rect.y, 0);
            var width  = Min(rect.width,  world.Width  - rect.x);
            var length = Min(rect.height, world.Length - rect.y);

            var meshFilter   = GetComponent<MeshFilter>();
            var meshRenderer = GetComponent<MeshRenderer>();
            var collider     = GetComponent<MeshCollider>();

            var meshCache = await Task.Factory.StartNew(()
            => TerrainMeshGenerator.GenerateChunkMesh(world, new(x, y, width, length)));

            var mesh = meshCache.ToMesh();

            meshRenderer.shadowCastingMode = ShadowCastingMode.TwoSided;
            meshRenderer.material          = Resources.Load<Material>("Materials/Terrain");

            meshFilter.mesh     = mesh;
            collider.sharedMesh = mesh;
        }

        async void Start()
        {
            await GenerateMesh();

            loaded = true;
        }
    }
}