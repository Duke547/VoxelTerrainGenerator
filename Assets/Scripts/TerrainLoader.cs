using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Profiling;
using UnityEngine;
using VoxelWorld.Classes;

using static System.Math;

namespace VoxelWorld.Scripts
{
    record LoadedChunk
    {
        public Vector2Int ChunkIndex { get; }

        public GameObject GameObject { get; }

        public LoadedChunk(Vector2Int chunkIndex, GameObject chunkObject)
        {
            ChunkIndex  = chunkIndex;
            GameObject = chunkObject;
        }
    }

    public sealed class TerrainLoader : MonoBehaviour
    {
        public int   ChunkSize       = 10;
        public int   ChunkCount      = 8;
        public float SecondsPerChunk = 1;

        public World World { get; set; }

        List<LoadedChunk> LoadedChunks { get; set; } = new();

        float SecondsUntilNextChunk { get; set; }

        Vector3? GetCurrentCameraPosition()
        {
            using (new ProfilerMarker($"{nameof(TerrainLoader)}.{nameof(GetCurrentCameraPosition)}").Auto())
            {
                var camera = FindObjectOfType<Camera>();

                if (camera != null)
                    return camera.transform.position;
                else
                    return null;
            }
        }

        Vector2Int GetChunkIndexAt(Vector3 location)
        {
            using (new ProfilerMarker($"{nameof(TerrainLoader)}.{nameof(GetChunkIndexAt)}").Auto())
            {
                var x = Max(0, (int)(location.x / ChunkSize));
                var y = Max(0, (int)(location.z / ChunkSize));

                return new(x, y);
            }
        }

        Vector2Int[] GetSurroundingChunkIndices(Vector3 location)
        {
            using (new ProfilerMarker($"{nameof(TerrainLoader)}.{nameof(GetSurroundingChunkIndices)}").Auto())
            {
                var chunks = new List<Vector2Int>();

                var center = GetChunkIndexAt(location);
                var x      = Max(0, center.x - ChunkCount);
                var y      = Max(0, center.y - ChunkCount);
                var width  = Min(ChunkCount * 2, World.Width  / ChunkSize - x);
                var height = Min(ChunkCount * 2, World.Length / ChunkSize - y);

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

        RectInt GetChunkRect(Vector2Int index)
        {
            using (new ProfilerMarker($"{nameof(TerrainLoader)}.{nameof(GetChunkRect)}").Auto())
            {
                return new(index.x * ChunkSize, index.y * ChunkSize, ChunkSize, ChunkSize);
            }
        }

        bool LoadChunk(Vector2Int chunkIndex)
        {
            var loadedIndices = LoadedChunks.Select(lc => lc.ChunkIndex);
            
            if (!loadedIndices.Contains(chunkIndex))
            {
                var rect = GetChunkRect(chunkIndex);
                var obj  = TerrainGenerator.GenerateChunk(World, rect);

                LoadedChunks.Add(new(chunkIndex, obj));

                return true;
            }

            return false;
        }

        public bool LoadChunk(Vector3 location)
            => LoadChunk(GetChunkIndexAt(location));

        void LoadCameraChunk()
        {
            var viewpoint = GetCurrentCameraPosition();

            if (viewpoint.HasValue)
                LoadChunk(viewpoint.Value);
        }

        void LoadNextChunk()
        {
            using (new ProfilerMarker($"{nameof(TerrainLoader)}.{nameof(LoadNextChunk)}").Auto())
            {
                var viewpoint = GetCurrentCameraPosition();

                if (viewpoint.HasValue)
                {
                    var desiredChunks = GetSurroundingChunkIndices(viewpoint.Value);

                    foreach (var desiredChunk in desiredChunks)
                    {
                        if (LoadChunk(desiredChunk))
                            break;
                    }
                }
            }
        }

        void DestroyChunk(LoadedChunk loadedChunk)
        {
            using (new ProfilerMarker($"{nameof(TerrainLoader)}.{nameof(DestroyChunk)}").Auto())
            {
                Destroy(loadedChunk.GameObject);

                LoadedChunks.Remove(loadedChunk);
            }
        }

        void UnloadChunks()
        {
            using (new ProfilerMarker($"{nameof(TerrainLoader)}.{nameof(UnloadChunks)}").Auto())
            {
                var viewpoint = GetCurrentCameraPosition();

                if (viewpoint.HasValue)
                {
                    var desiredChunkIndices = GetSurroundingChunkIndices(viewpoint.Value);

                    foreach (var loadedChunk in LoadedChunks.ToArray())
                    {
                        if (!desiredChunkIndices.Contains(loadedChunk.ChunkIndex))
                            DestroyChunk(loadedChunk);
                    }
                }
            }
        }

        void Start() =>
            SecondsUntilNextChunk = SecondsPerChunk;

        void Update()
        {
            SecondsUntilNextChunk -= Time.deltaTime;

            UnloadChunks();
            LoadCameraChunk();

            if (SecondsUntilNextChunk <= 0)
            {
                LoadNextChunk();

                SecondsUntilNextChunk = SecondsPerChunk;
            }
        }
    }
}