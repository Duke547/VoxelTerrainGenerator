using System.Collections.Generic;
using System.Linq;
using Unity.Profiling;
using UnityEngine;
using VoxelWorld.Classes;

using static System.Math;

namespace VoxelWorld.Scripts
{
    public sealed class TerrainLoader : MonoBehaviour
    {
        public int chunkSize  = 10;
        public int chunkCount = 8;

        private List<TerrainChunk> chunks { get; set; } = new List<TerrainChunk>();

        public World world { get; set; }

        private Vector2Int GetChunkIndexAt(Vector3 position)
        {
            using (new ProfilerMarker($"{nameof(TerrainLoader)}.{nameof(GetChunkIndexAt)}").Auto())
            {
                var x = Max(0, (int)(position.x / chunkSize));
                var y = Max(0, (int)(position.z / chunkSize));

                return new(x, y);
            }
        }

        private WorldChunk GetChunkAt(Vector2Int chunkIndex)
            => new(world, chunkIndex, chunkSize);

        private WorldChunk GetChunkAt(Vector3 position)
            => GetChunkAt(GetChunkIndexAt(position));

        private WorldChunk[] GetChunkRegion(Vector3 viewpoint)
        {
            using (new ProfilerMarker($"{nameof(TerrainLoader)}.{nameof(GetChunkRegion)}").Auto())
            {
                var chunks = new List<WorldChunk>();

                if (world == null)
                    return chunks.ToArray();

                var center = GetChunkIndexAt(viewpoint);
                var start  = center - new Vector2Int(chunkCount,     chunkCount    );
                var end    = start  + new Vector2Int(chunkCount * 2, chunkCount * 2);

                for (var y = start.y; y < end.y; y++)
                {
                    for (var x = start.x; x < end.x; x++)
                        chunks.Add(GetChunkAt(new Vector2Int(x, y)));
                }

                return chunks
                    .OrderBy(chunk => Vector2Int.Distance(center, chunk.index))
                    .ToArray();
            }
        }

        private bool LoadChunk(WorldChunk chunk)
        {
            using (new ProfilerMarker($"{nameof(TerrainLoader)}.{nameof(LoadChunk)}").Auto())
            {
                if (chunks.Any(terrainChunk => terrainChunk.worldChunk == chunk))
                    return false;

                var chunkObject  = new GameObject($"Terrain Chunk {chunk.index}");
                var terrainChunk = chunkObject.AddComponent<TerrainChunk>();

                chunkObject.transform.SetParent(transform);

                terrainChunk.worldChunk = chunk;

                chunks.Add(terrainChunk);

                return true;
            }
        }

        public bool LoadChunk(Vector3 position)
            => LoadChunk(GetChunkAt(position));

        private bool RemoveChunk(TerrainChunk chunk)
        {
            if (!chunk.isLoaded)
                return false;

            chunks.Remove(chunk);

            Destroy(chunk.gameObject);

            return true;
        }

        private void LoadNextChunk()
        {
            using (new ProfilerMarker($"{nameof(TerrainLoader)}.{nameof(LoadNextChunk)}").Auto())
            {
                var camera = FindObjectOfType<Camera>();

                if (camera != null)
                {
                    var viewpoint = camera.transform.position;
                    var region    = GetChunkRegion(viewpoint);

                    foreach (var chunk in region)
                    {
                        if (LoadChunk(chunk))
                            break;
                    }
                }
            }
        }

        private void RemoveNextChunk()
        {
            using (new ProfilerMarker($"{nameof(TerrainLoader)}.{nameof(RemoveNextChunk)}").Auto())
            {
                var camera = FindObjectOfType<Camera>();

                if (camera != null)
                {
                    var viewpoint = camera.transform.position;
                    var region    = GetChunkRegion(viewpoint);

                    foreach (var chunk in chunks)
                    {
                        if (!region.Any(c => c == chunk.worldChunk))
                        {
                            if (RemoveChunk(chunk))
                                break;
                        }
                    }
                }
            }
        }

        private void Update()
        {
            if (world != null && Time.frameCount % 10 == 0)
            {
                LoadNextChunk();
                RemoveNextChunk();
            }
        }
    }
}