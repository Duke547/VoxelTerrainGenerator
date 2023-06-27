using System.IO;
using System.Text;
using NUnit.Framework;
using UnityEngine;
using VoxelWorld.Classes;

namespace VoxelWorld.Testing
{
    public class MeshCacheTests
    {
        [Test]
        public void ToMesh_Test()
        {
            var meshCache = new MeshCache();
            var vertex    = new Vertex()
            {
                Position = new Vector3Int(1, 2, 3),
                Normal   = Vector3.up,
                UV       = Vector2.one,
                Color    = Color  .red
            };
            
            meshCache.Vertices.Add(vertex);
            meshCache.Indices .Add(0);
            meshCache.Indices .Add(0);
            meshCache.Indices .Add(0);
            meshCache.Topology = MeshTopology.Lines;

            var mesh = meshCache.ToMesh();

            Assert.That(mesh.vertices[0],    Is.EqualTo(vertex.Position   ));
            Assert.That(mesh.normals [0],    Is.EqualTo(vertex.Normal     ));
            Assert.That(mesh.uv      [0],    Is.EqualTo(vertex.UV         ));
            Assert.That(mesh.colors  [0],    Is.EqualTo(vertex.Color      ));
            Assert.That(mesh.GetTopology(0), Is.EqualTo(MeshTopology.Lines));
        }

        [Test]
        public void ReadWrite_Test()
        {
            var file  = ".//Assets/Tests/TestData/Mesh";
            var input = new MeshCache();

            input.Vertices.Add(new Vertex() { Position = new Vector3(1, 2, 3), Normal = Vector3.up,      Color = Color.red,   UV = Vector2.one });
            input.Vertices.Add(new Vertex() { Position = new Vector3(4, 5, 6), Normal = Vector3.down,    Color = Color.blue,  UV = Vector2.zero});
            input.Vertices.Add(new Vertex() { Position = new Vector3(7, 8, 9), Normal = Vector3.forward, Color = Color.green, UV = Vector2.one });

            input.Indices.Add(0);
            input.Indices.Add(1);
            input.Indices.Add(2);

            MeshCache output;

            using var writeStream = File.Open(file, FileMode.Create);
            {
                using var writer = new BinaryWriter(writeStream, Encoding.UTF8, false);
                {
                    MeshCache.Write(writer, input);
                }
            }

            using var readStream = File.Open(file, FileMode.Open);
            {
                using var reader = new BinaryReader(readStream, Encoding.UTF8, false);
                {
                    output = MeshCache.Read(reader);
                }
            }

            Assert.That(output.Vertices, Is.EquivalentTo(input.Vertices), "Vertices");
            Assert.That(output.Indices,  Is.EquivalentTo(input.Indices),  "Indices" );
            Assert.That(output.Topology, Is.EqualTo     (input.Topology), "Topology");
        }
    }
}