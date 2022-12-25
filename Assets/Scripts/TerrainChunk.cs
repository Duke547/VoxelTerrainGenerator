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

        public bool isLoaded { get; private set; }

        private async void GenerateMesh()
        {
            if (worldChunk == null)
                return;

            isLoaded = false;

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

            isLoaded = true;
        }

        private void Start()
            => GenerateMesh();
    }
}