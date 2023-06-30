using NUnit.Framework;
using UnityEngine;
using VoxelWorld.MonoBehaviors;

namespace VoxelWorld.Testing
{
    public class BlockTargetTests
    {
        [Test]
        public void Constructor_Test()
        {
            var location           = new Vector3Int(1, 2, 3);
            var terrainChunkObject = new GameObject();
            var terrainChunk       = terrainChunkObject.AddComponent<TerrainChunk>();
            var blockTarget        = new BlockTarget(location, terrainChunk);

            Assert.That(blockTarget.Position,     Is.EqualTo(location));
            Assert.That(blockTarget.TerrainChunk, Is.EqualTo(terrainChunk));
        }
    }
}