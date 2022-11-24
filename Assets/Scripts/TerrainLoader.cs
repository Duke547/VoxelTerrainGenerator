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

        public GameObject ChunkObject { get; }

        public LoadedChunk(Vector2Int chunkIndex, GameObject chunkObject)
        {
            ChunkIndex  = chunkIndex;
            ChunkObject = chunkObject;
        }
    }

    public sealed class TerrainLoader : MonoBehaviour
    {
        public int   ChunkSize       = 10;
        public int   ChunkCount      = 8;
        public float SecondsPerChunk = 1;

        public World World { get; set; }

        List<LoadedChunk> LoadedChunks { get; set; } = new();

        List<Vector2Int> ScheduledChunks { get; set; } = new();

        List<Vector2Int> ObsoleteChunks { get; set; } = new();

        float SecondsUntilNextChunk { get; set; }

        Vector2Int GetChunkIndexAt(Vector3 location)
        {
            using (new ProfilerMarker($"{nameof(TerrainLoader)}.LoadChunks").Auto())
            {
                var x = Max(0, (int)(location.x / ChunkSize));
                var y = Max(0, (int)(location.z / ChunkSize));

                return new(x, y);
            }
        }

        Vector2Int[] GetSurroundingChunkIndices(Vector3 location)
        {
            using (new ProfilerMarker($"{nameof(TerrainLoader)}.GetSurroundingChunkIndices").Auto())
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

        RectInt GetChunkBounds(Vector2Int index)
        {
            using (new ProfilerMarker($"{nameof(TerrainLoader)}.GetChunkBounds").Auto())
            {
                return new(index.x * ChunkSize, index.y * ChunkSize, ChunkSize, ChunkSize);
            }
        }

        void CreateChunk(Vector2Int index)
        {
            var bounds = GetChunkBounds(index);
            var obj    = TerrainGenerator.GenerateChunk(World, bounds);

            LoadedChunks.Add(new(index, obj));
        }

        public void CreateChunk(Vector3 location)
            => CreateChunk(GetChunkIndexAt(location));

        void ScheduleChunks(Vector3 viewpoint)
        {
            using (new ProfilerMarker($"{nameof(TerrainLoader)}.{nameof(ScheduleChunks)}").Auto())
            {
                var current  = GetChunkIndexAt(viewpoint);

                if (!LoadedChunks.Any(c => c.ChunkIndex == current))
                    CreateChunk(current);

                var desiredChunks   = GetSurroundingChunkIndices(viewpoint);
                var scheduledChunks = ScheduledChunks.ToArray();
                var obsoleteChunks  = ObsoleteChunks .ToArray();
                var loadedChunks    = LoadedChunks   .ToArray();

                foreach (var scheduledChunkIndex in scheduledChunks)
                {
                    if (!desiredChunks.Contains(scheduledChunkIndex))
                        ScheduledChunks.Remove(scheduledChunkIndex);
                }

                foreach (var obsoleteChunkIndex in obsoleteChunks)
                {
                    if (desiredChunks.Contains(obsoleteChunkIndex))
                        ObsoleteChunks.Remove(obsoleteChunkIndex);
                }

                foreach (var loadedChunk in loadedChunks)
                {
                    if (!desiredChunks.Contains(loadedChunk.ChunkIndex) && !ObsoleteChunks.Contains(loadedChunk.ChunkIndex))
                        ObsoleteChunks.Add(loadedChunk.ChunkIndex);
                }

                foreach (var desiredChunkIndex in desiredChunks)
                {
                    if (!ScheduledChunks.Contains(desiredChunkIndex) && !LoadedChunks.Any(c => c.ChunkIndex == desiredChunkIndex))
                        ScheduledChunks.Add(desiredChunkIndex);
                }

                ScheduledChunks.Remove(current);
            }
        }

        void CreateNextScheduledChunk()
        {
            if (ScheduledChunks.Count > 0)
            {
                CreateChunk(ScheduledChunks[0]);
                ScheduledChunks.RemoveAt(0);
            }
        }

        void DestroyChunk(Vector2Int index)
        {
            var chunk = LoadedChunks.Find(c => c.ChunkIndex == index);

            Destroy(chunk.ChunkObject);

            LoadedChunks.Remove(chunk);
        }

        void DestroyObsoleteChunks()
        {
            using (new ProfilerMarker($"{nameof(TerrainLoader)}.{nameof(DestroyObsoleteChunks)}").Auto())
            {
                var chunks = ObsoleteChunks.ToArray();

                foreach (var chunkIndex in chunks)
                {
                    DestroyChunk(chunkIndex);

                    ObsoleteChunks.Remove(chunkIndex);
                }
            }
        }

        void Start()
        {
            SecondsUntilNextChunk = SecondsPerChunk;
        }

        void Update()
        {
            SecondsUntilNextChunk -= Time.deltaTime;

            var player = GameObject.FindWithTag("Player");

            if (player != null)
                ScheduleChunks(player.transform.position);

            DestroyObsoleteChunks();

            if (SecondsUntilNextChunk <= 0)
            {
                CreateNextScheduledChunk();

                SecondsUntilNextChunk = SecondsPerChunk;
            }
        }
    }
}