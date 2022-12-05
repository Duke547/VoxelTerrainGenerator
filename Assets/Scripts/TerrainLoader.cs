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

        internal bool[,] chunks { get; private set; }

        World _world;

        public World world
        {
            get => _world;
            set
            {
                _world = value;
                chunks = new bool[value.Width / chunkSize, value.Length / chunkSize];
            }
        }

        public Vector2Int GetChunkIndexAt(Vector3 location)
        {
            using (new ProfilerMarker($"{nameof(TerrainLoader)}.{nameof(GetChunkIndexAt)}").Auto())
            {
                var x = Max(0, (int)(location.x / chunkSize));
                var y = Max(0, (int)(location.z / chunkSize));

                return new(x, y);
            }
        }

        public Vector2Int[] GetSurroundingChunkIndices(Vector3 location)
        {
            using (new ProfilerMarker($"{nameof(TerrainLoader)}.{nameof(GetSurroundingChunkIndices)}").Auto())
            {
                var chunks = new List<Vector2Int>();

                var center = GetChunkIndexAt(location);
                var x      = Max(0, center.x - chunkCount);
                var y      = Max(0, center.y - chunkCount);
                var width  = Min(chunkCount * 2, world.Width  / chunkSize - x);
                var height = Min(chunkCount * 2, world.Length / chunkSize - y);

                for (var yIndex = 0; yIndex < height; yIndex++)
                {
                    for (var xIndex = 0; xIndex < width; xIndex++)
                        chunks.Add(new(x + xIndex, y + yIndex));
                }

                return chunks
                    .OrderBy(c => Vector2Int.Distance(center, c))
                    .ToArray();
            }
        }

        public RectInt GetChunkRect(Vector2Int index)
        {
            using (new ProfilerMarker($"{nameof(TerrainLoader)}.{nameof(GetChunkRect)}").Auto())
            {
                return new(index.x * chunkSize, index.y * chunkSize, chunkSize, chunkSize);
            }
        }

        bool LoadChunk(Vector2Int chunkIndex)
        {
            using (new ProfilerMarker($"{nameof(TerrainLoader)}.{nameof(LoadChunk)}").Auto())
            {
                if (!chunks[chunkIndex.x, chunkIndex.y])
                {
                    var rect        = GetChunkRect(chunkIndex);
                    var chunkObject = new GameObject($"Terrain Chunk {chunkIndex}");
                    var chunk       = chunkObject.AddComponent<TerrainChunk>();

                    chunkObject.transform.SetParent(transform);

                    chunk.terrainLoader = this;
                    chunk.chunkIndex    = chunkIndex;

                    chunks[chunkIndex.x, chunkIndex.y] = true;

                    return true;
                }

                return false;
            }
        }

        public bool LoadChunk(Vector3 location)
            => LoadChunk(GetChunkIndexAt(location));

        void LoadNextChunk()
        {
            using (new ProfilerMarker($"{nameof(TerrainLoader)}.{nameof(LoadNextChunk)}").Auto())
            {
                var camera = FindObjectOfType<Camera>();

                if (camera != null)
                {
                    var viewpoint            = camera.transform.position;
                    var desiredChunksIndices = GetSurroundingChunkIndices(viewpoint);

                    foreach (var desiredChunkIndex in desiredChunksIndices)
                    {
                        if (LoadChunk(desiredChunkIndex))
                            break;
                    }
                }
            }
        }

        void Update()
        {
            if (world != null)
                LoadNextChunk();
        }
    }
}