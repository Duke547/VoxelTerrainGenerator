using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace VoxelWorld.Classes
{
    public class MeshCache
    {
        public List<Vertex> Vertices { get; } = new();

        public List<int> Indices { get; } = new();

        public MeshTopology topology { get; set; } = MeshTopology.Triangles;

        public Mesh ToMesh()
        {
            var mesh     = new Mesh() { indexFormat = IndexFormat.UInt32 };
            var vertices = new List<Vector3>();
            var normals  = new List<Vector3>();
            var uvs      = new List<Vector2>();
            var colors   = new List<Color>();

            foreach (var vertex in Vertices)
            {
                vertices.Add(vertex.Position);
                normals .Add(vertex.Normal  );
                uvs     .Add(vertex.UV      );
                colors  .Add(vertex.Color   );
            }

            mesh.SetVertices(vertices            );
            mesh.SetIndices (Indices, topology, 0);
            mesh.SetNormals (normals             );
            mesh.SetUVs     (0, uvs              );
            mesh.SetColors  (colors              );

            return mesh;
        }
    }
}
