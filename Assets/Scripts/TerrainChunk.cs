using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using VoxelWorld.Classes;

using static System.Math;

namespace VoxelWorld.Scripts
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
    public class TerrainChunk : MonoBehaviour
    {
        TerrainLoader _terrainLoader;

        internal TerrainLoader terrainLoader
        {
            get
            {
                if (_terrainLoader == null)
                    throw new InvalidOperationException("Parent terrain loader no longer exists.");

                return _terrainLoader;
            }
            set => _terrainLoader = value;
        }

        World world
        {
            get
            {
                var world = terrainLoader.world;

                if (world == null)
                    throw new InvalidOperationException("Terrain loader does not have a world.");

                return world;
            }
        }

        public Vector2Int chunkIndex { get; internal set; }

        public bool loaded { get; private set; }

        async Task GenerateMesh()
        {
            var rect   = terrainLoader.GetChunkRect(chunkIndex);
            var x      = Max(rect.x, 0);
            var y      = Max(rect.y, 0);
            var width  = Min(rect.width,  world.Width  - rect.x);
            var length = Min(rect.height, world.Length - rect.y);

            var meshFilter   = GetComponent<MeshFilter>();
            var meshRenderer = GetComponent<MeshRenderer>();
            var collider     = GetComponent<MeshCollider>();

            var meshCache = await Task.Factory.StartNew(() =>
                TerrainMeshGenerator.GenerateChunkMesh(world, new(x, y, width, length)));

            var mesh = meshCache.ToMesh();

            meshRenderer.shadowCastingMode = ShadowCastingMode.TwoSided;
            meshRenderer.material          = Resources.Load<Material>("Materials/Terrain");

            meshFilter.mesh     = mesh;
            collider.sharedMesh = mesh;
        }

        void Remove()
        {
            Destroy(gameObject);

            terrainLoader.chunks[chunkIndex.x, chunkIndex.y] = false;
        }

        void CheckForRemoval()
        {
            if (!loaded)
                return;

            var camera = FindObjectOfType<Camera>();

            if (camera == null)
                return;

            var cameraPos = camera.transform.position;
            var maxDist   = terrainLoader.chunkCount * terrainLoader.chunkSize;
            var distRect  = new RectInt((int)cameraPos.x - maxDist, (int)cameraPos.z - maxDist, maxDist*2, maxDist*2);
            var chunkRect = terrainLoader.GetChunkRect(chunkIndex);

            if (!distRect.Overlaps(chunkRect))
                Remove();
        }

        async void Start()
        {
            await GenerateMesh();

            loaded = true;
        }

        void Update() =>
            CheckForRemoval();
    }
}