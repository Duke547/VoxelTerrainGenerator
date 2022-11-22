using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

using UnityMesh = UnityEngine.Mesh;

namespace VoxelWorld.Classes
{
    public record Vertex
    {
        public Vector3 Location { get; set; }

        public Color Color { get; set; } = Color.white;

        public static implicit operator Vector3(Vertex vertex)
            => vertex.Location;

        public Vertex(Vector3 location)
            => Location = location;
    }

    public class Mesh
    {
        public List<Vertex> Vertices { get; } = new();

        public List<int> Indices { get; } = new();

        public List<Vector3> Normals { get; } = new();

        public List<Vector2> UVs { get; } = new();

        public Color[] Colors => Vertices
            .Select(v => v.Color)
            .ToArray();

        public UnityMesh ToUnityMesh()
        {
            var mesh = new UnityMesh() { indexFormat = IndexFormat.UInt32};

            mesh.SetVertices(Vertices.Select(v => v.Location).ToArray());
            mesh.SetIndices(Indices, MeshTopology.Triangles, 0);
            mesh.SetNormals(Normals);
            mesh.SetUVs(0, UVs);
            mesh.SetColors(Colors);

            return mesh;
        }
    }
}
