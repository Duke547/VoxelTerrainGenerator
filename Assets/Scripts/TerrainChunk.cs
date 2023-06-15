using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using VoxelWorld.Classes;

namespace VoxelWorld.Scripts
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
    public class TerrainChunk : MonoBehaviour
    {
        public WorldChunk worldChunk { get; set; }

        public bool isLoading { get; private set; }

        public bool refresh { get; set; } = true;

        public async void GenerateMesh()
        {
            if (worldChunk == null || isLoading)
                return;

            isLoading = true;

            var meshCache = await Task.Run(() => TerrainMeshGenerator.GenerateChunkMesh(worldChunk.world, worldChunk.area));

            if (this == null)
                return;

            var meshFilter   = GetComponent<MeshFilter>();
            var meshRenderer = GetComponent<MeshRenderer>();
            var collider     = GetComponent<MeshCollider>();
            var mesh         = meshCache.ToMesh();

            meshRenderer.shadowCastingMode = ShadowCastingMode.TwoSided;
            meshRenderer.material          = Resources.Load<Material>("Materials/Terrain");

            meshFilter.mesh     = mesh;
            collider.sharedMesh = mesh;

            isLoading = false;
        }
    }
}