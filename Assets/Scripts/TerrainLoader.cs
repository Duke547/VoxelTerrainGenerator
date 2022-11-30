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
        public int   ChunkSize  = 10;
        public int   ChunkCount = 8;

        public World World { get; set; }

        List<LoadedChunk> LoadedChunks { get; set; } = new();

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

        Bounds GetChunkBounds(Vector2Int chunkIndex)
        {
            var chunkRect         = GetChunkRect(chunkIndex);
            var chunkRectCenter   = chunkRect.center;
            var chunkBoundsCenter = new Vector3(chunkRectCenter.x, World.Height / 2, chunkRectCenter.y);

            return new Bounds(chunkBoundsCenter, new(chunkRect.width, World.Height, chunkRect.height));
        }

        bool ChunkInView(Vector2Int chunkIndex)
        {
            var camera = FindObjectOfType<Camera>();

            if (camera != null)
            {
                var chunkBounds = GetChunkBounds(chunkIndex);
                var viewPlanes  = GeometryUtility.CalculateFrustumPlanes(camera);

                return GeometryUtility.TestPlanesAABB(viewPlanes, chunkBounds);
            }
            else
            {
                return false;
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
                        if (ChunkInView(desiredChunkIndex))
                        {
                            if (LoadChunk(desiredChunkIndex))
                                break;
                        }
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

        void UnloadNextChunk()
        {
            using (new ProfilerMarker($"{nameof(TerrainLoader)}.{nameof(UnloadNextChunk)}").Auto())
            {
                var camera = FindObjectOfType<Camera>();

                if (camera != null)
                {
                    var viewpoint = camera.transform.position;

                    var desiredChunkIndices = GetSurroundingChunkIndices(viewpoint);

                    for (var i = 0; i < LoadedChunks.Count; i++)
                    {
                        var loadedChunk = LoadedChunks[i];

                        if (!desiredChunkIndices.Contains(loadedChunk.ChunkIndex))
                        {
                            DestroyChunk(loadedChunk);
                            break;
                        }
                    }
                }
            }
        }

        void Update()
        {
            UnloadNextChunk();
            LoadNextChunk();
        }
    }
}