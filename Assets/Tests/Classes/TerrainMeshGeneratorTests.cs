using System.IO;
using NUnit.Framework;
using UnityEngine;
using VoxelWorld.Classes;

namespace VoxelWorld.Testing
{
    public class TerrainMeshGeneratorTests
    {
        [Test]
        public void GenerateChunkMesh_Test()
        {
            var blocks = new byte[4,4,4];
            var world  = new World(blocks);
            var chunk  = new WorldChunk(world, Vector2Int.zero, 3);
            var center = new Vector3Int(1, 1, 1);

            world.SetBlock(center);
            world.SetBlock(center + Vector3Int.up     );
            world.SetBlock(center + Vector3Int.down   );
            world.SetBlock(center + Vector3Int.left   );
            world.SetBlock(center + Vector3Int.right  );
            world.SetBlock(center + Vector3Int.forward);
            world.SetBlock(center + Vector3Int.back   );

            var mesh = TerrainMeshGenerator.GenerateChunkMesh(chunk);

            //using var stream = new FileStream(".//Assets/Tests/TestData/TerrainTest", FileMode.Create);
            //using var writer = new BinaryWriter(stream);

            //MeshCache.Write(writer, mesh);

            using var stream = new FileStream(".//Assets/Tests/TestData/TerrainTest", FileMode.Open);
            using var reader = new BinaryReader(stream);
            
            var expectedMesh = MeshCache.Read(reader);

            Assert.That(mesh.Vertices, Is.EquivalentTo(expectedMesh.Vertices), "Vertices");
            Assert.That(mesh.Indices,  Is.EquivalentTo(expectedMesh.Indices ), "Normals" );
        }
    }
}