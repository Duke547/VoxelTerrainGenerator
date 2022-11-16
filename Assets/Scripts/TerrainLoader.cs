using System.Collections.Generic;
using System.Linq;
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
        List<LoadedChunk> LoadedChunks { get; set; } = new();

        Vector2Int CurrentChunk { get; set; }

        public World World { get; set; }

        public int ChunkSize = 10;

        public int ChunkCount = 8;

        Vector2Int GetChunkIndexAt(Vector3 location)
        {
            var x = Max(0, (int)(location.x / ChunkSize));
            var y = Max(0, (int)(location.z / ChunkSize));

            return new(x, y);
        }

        Vector2Int[] GetSurroundingChunkIndices(Vector3 location)
        {
            var chunks = new List<Vector2Int>();

            var center = GetChunkIndexAt(location);
            var x      = Max(0,                        center.x - ChunkCount);
            var y      = Max(0,                        center.y - ChunkCount);
            var width  = Min(World.Width  / ChunkSize, x + center.x + ChunkCount);
            var height = Min(World.Length / ChunkSize, y + center.y + ChunkCount);

            for (var yIndex = y; yIndex < height; yIndex++)
            {
                for (var xIndex = x; xIndex < width; xIndex++)
                    chunks.Add(new(xIndex, yIndex));
            }

            return chunks.ToArray();
        }

        RectInt GetChunkBounds(Vector2Int index) =>
            new(index.x * ChunkSize, index.y * ChunkSize, ChunkSize, ChunkSize);

        public void LoadChunks(Vector3 viewpoint)
        {
            var chunks    = GetSurroundingChunkIndices(viewpoint);
            var newChunks = chunks.Where(c => !LoadedChunks.Any(lc => lc.ChunkIndex == c)).ToArray();
            var oldChunks = LoadedChunks.Where(lc => !chunks.Any(c => c == lc.ChunkIndex)).ToArray();

            foreach (var newChunk in newChunks)
            {
                var chunkBounds = GetChunkBounds(newChunk);
                var obj         = TerrainGenerator.GenerateChunk(World, chunkBounds);

                LoadedChunks.Add(new(newChunk, obj));
            }

            foreach (var oldChunk in oldChunks)
            {
                Destroy(oldChunk.ChunkObject);
                
                LoadedChunks.Remove(oldChunk);
            }

            CurrentChunk = GetChunkIndexAt(viewpoint);
        }

        void Update()
        {
            var camera = FindObjectOfType<Camera>();

            if (camera != null)
            {
                var viewpoint         = camera.transform.position;
                var updatedChunkIndex = GetChunkIndexAt(viewpoint);

                if (updatedChunkIndex != CurrentChunk)
                {
                    LoadChunks(viewpoint);
                }
            }
        }
    }
}