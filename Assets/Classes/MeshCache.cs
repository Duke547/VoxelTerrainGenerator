using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace VoxelWorld.Classes
{
    public class MeshCache
    {
        public Dictionary<int, Vector3> Vertices { get; } = new();

        public List<int> Indices { get; } = new();

        public Dictionary<int, Vector3> Normals { get; } = new();

        public Dictionary<int, Vector2> UVs { get; } = new();

        public Dictionary<int, Color> Colors { get; } = new();

        public MeshTopology topology { get; set; } = MeshTopology.Triangles;

        public Mesh ToMesh()
        {
            var mesh = new Mesh() { indexFormat = IndexFormat.UInt32 };

            mesh.SetVertices(Vertices.Values.ToArray());
            mesh.SetNormals (Normals .Values.ToArray());
            mesh.SetUVs     (0, UVs  .Values.ToArray());
            mesh.SetColors  (Colors  .Values.ToArray());
            
            mesh.SetIndices (Indices, topology, 0);

            return mesh;
        }
    }
}
