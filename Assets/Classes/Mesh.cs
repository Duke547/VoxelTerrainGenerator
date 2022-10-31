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

    public record Edge
    {
        public Vertex V1 { get; }
        
        public Vertex V2 { get; }

        public Edge(Vertex v1, Vertex v2)
        {
            V1 = v1;
            V2 = v2;
        }
    }

    public record Triangle
    {
        public Vertex V1 { get; }
        
        public Vertex V2 { get; }
        
        public Vertex V3 { get; }

        public Vertex[] Vertices => new[] { V1, V2, V3 }; 

        public Vector3 Normal { get; }

        public Edge GetSharedEdge(Triangle other)
        {
            var shared = this.Vertices.Intersect(other.Vertices);

            if (shared.Count() == 2)
                return new Edge(shared.First(), shared.Last());
            else
                return null;
        }

        public bool Adjacent(Triangle other)
            => GetSharedEdge(other) != null;

        public Triangle(Vertex v1, Vertex v2, Vertex v3, Vector3 normal)
        {
            V1     = v1;
            V2     = v2;
            V3     = v3;
            Normal = normal;
        }
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

        public Triangle[] Triangles
        {
            get
            {
                var triangles = new List<Triangle>();

                for (int i = 0; i < Indices.Count; i += 3)
                {
                    var i1 = Indices[i];
                    var i2 = Indices[i+1];
                    var i3 = Indices[i+2];

                    var v1 = Vertices[i1];
                    var v2 = Vertices[i2];
                    var v3 = Vertices[i3];

                    var n1 = Normals[i1];
                    var n2 = Normals[i1];
                    var n3 = Normals[i1];

                    var normal = (n1 + n2 + n3) / 3;

                    triangles.Add(new(v1, v2, v3, normal));
                }

                return triangles.ToArray();
            }
        }

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
