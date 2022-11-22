using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace VoxelWorld.Classes
{
    public class MeshCache
    {
        public List<Vector3> Vertices { get; } = new();

        public List<int> Indices { get; } = new();

        public List<Vector3> Normals { get; } = new();

        public List<Vector2> UVs { get; } = new();

        public List<Color> Colors { get; } = new();

        public Mesh ToMesh()
        {
            var mesh = new Mesh() { indexFormat = IndexFormat.UInt32};

            mesh.SetVertices(Vertices);
            mesh.SetIndices(Indices, MeshTopology.Triangles, 0);
            mesh.SetNormals(Normals);
            mesh.SetUVs(0, UVs);
            mesh.SetColors(Colors);

            return mesh;
        }
    }
}
