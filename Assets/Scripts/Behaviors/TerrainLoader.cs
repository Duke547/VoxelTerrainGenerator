using System.Collections.Generic;
using UnityEngine;
using VoxelWorld.Classes;

using static System.Math;

namespace VoxelWorld.Scripts
{
    public sealed class TerrainLoader : MonoBehaviour
    {
        public int chunkSize  = 10;
        public int chunkCount = 8;

        Dictionary<Vector2Int, TerrainChunk> chunks { get; set; } = new();

        public World world { get; set; }

        private Vector2Int GetChunkIndexAt(Vector3 position)
        {
            var x = Max(0, (int)(position.x / chunkSize));
            var y = Max(0, (int)(position.z / chunkSize));

            return new(x, y);
        }

        private WorldChunk GetChunkAt(Vector2Int chunkIndex)
            => new(world, chunkIndex, chunkSize);

        private WorldChunk GetChunkAt(Vector3 position)
            => GetChunkAt(GetChunkIndexAt(position));

        private Dictionary<Vector2Int, WorldChunk> GetChunkRegion(Vector3 viewpoint)
        {
            var chunks = new Dictionary<Vector2Int, WorldChunk>();

            if (world == null)
                return chunks;

            var center = GetChunkIndexAt(viewpoint);
            var start  = center - new Vector2Int(chunkCount,     chunkCount    );
            var end    = start  + new Vector2Int(chunkCount * 2, chunkCount * 2);

            for (var y = start.y; y < end.y; y++)
            {
                for (var x = start.x; x < end.x; x++)
                {
                    var index = new Vector2Int(x, y);
                    var chunk = GetChunkAt(index);

                    chunks.Add(index, chunk);
                }
            }

            return chunks;
        }

        private void LoadChunk(WorldChunk chunk)
        {
            var chunkObject  = new GameObject($"Terrain Chunk {chunk.Index}");
            var terrainChunk = chunkObject.AddComponent<TerrainChunk>();

            chunkObject.transform.SetParent(transform);

            terrainChunk.worldChunk = chunk;

            chunks.Add(chunk.Index, terrainChunk);

            terrainChunk.GenerateMesh();
        }

        public void LoadChunk(Vector3 position)
            => LoadChunk(GetChunkAt(position));

        private bool RemoveChunk(TerrainChunk chunk)
        {
            if (chunk.isLoading)
                return false;

            chunks.Remove(chunk.worldChunk.Index);

            Destroy(chunk.gameObject);

            return true;
        }

        private void LoadNextChunk()
        {
            if (PlayerCamera.current != null)
            {
                var viewpoint = PlayerCamera.current.transform.position;
                var region    = GetChunkRegion(viewpoint);

                foreach (var chunk in region)
                {
                    if (chunks.ContainsKey(chunk.Value.Index))
                    {
                        var terrainChunk = chunks[chunk.Value.Index];

                        if (terrainChunk.refresh && !terrainChunk.isLoading)
                        {
                            terrainChunk.GenerateMesh();
                            terrainChunk.refresh = false;

                            break;
                        }
                    }
                    else
                    {
                        LoadChunk(chunk.Value);
                        break;
                    }
                }
            }
        }

        private void RemoveNextChunk()
        {
            var camera = FindObjectOfType<Camera>();

            if (camera != null)
            {
                var viewpoint = camera.transform.position;
                var region    = GetChunkRegion(viewpoint);

                foreach (var chunk in chunks)
                {
                    if (!region.ContainsKey(chunk.Key))
                    {
                        if (RemoveChunk(chunk.Value))
                            break;
                    }
                }
            }
        }

        private void Update()
        {
            if (world != null)
            {
                LoadNextChunk();
                RemoveNextChunk();
            }
        }
    }
}