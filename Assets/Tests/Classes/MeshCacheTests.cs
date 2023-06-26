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
    }
}