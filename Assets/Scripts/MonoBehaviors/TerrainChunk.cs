using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;

namespace VoxelWorld.MonoBehaviors
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

            var meshCache = await Task.Run(() => TerrainMeshGenerator.GenerateChunkMesh(worldChunk));

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

        private TerrainChunk[] GetAdjacentChunks()
        {
                var adjacents    = new List<TerrainChunk>();
                var loadedChunks = FindObjectsOfType<TerrainChunk>();
                var directions   = new Vector2Int[]
                {
                    Vector2Int.left,
                    Vector2Int.left  + Vector2Int.up,
                    Vector2Int.up,
                    Vector2Int.up    + Vector2Int.right,
                    Vector2Int.right,
                    Vector2Int.right + Vector2Int.down,
                    Vector2Int.down,
                    Vector2Int.down  + Vector2Int.left
                };

                foreach (var direction in directions)
                {
                    var loadedChunk = loadedChunks.FirstOrDefault(lc => lc.worldChunk.Index == worldChunk.Index + direction);

                    if (loadedChunk != null)
                        adjacents.Add(loadedChunk);
                }

                return adjacents.ToArray();
        }

        public void Refresh()
        {
            refresh = true;

            foreach (var adjacentChunk in GetAdjacentChunks())
                adjacentChunk.refresh = true;
        }

        public void BreakBlock(Vector3Int position)
        {
            worldChunk.World.SetBlock(position, "Air");

            Refresh();
        }

        public void PlaceBlock(Vector3Int position)
        {
            if (!Physics.CheckBox(position, new(0.4f, 0.4f, 0.4f)))
            {
                worldChunk.World.SetBlock(position, "Dirt");

                Refresh();
            }
        }
    }
}