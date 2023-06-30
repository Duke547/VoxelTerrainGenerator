using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;

namespace VoxelWorld
{
    public class MeshCache
    {
        public List<Vertex> Vertices { get; } = new();

        public List<int> Indices { get; } = new();

        public MeshTopology Topology { get; set; } = MeshTopology.Triangles;

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
            mesh.SetIndices (Indices, Topology, 0);
            mesh.SetNormals (normals             );
            mesh.SetUVs     (0, uvs              );
            mesh.SetColors  (colors              );

            return mesh;
        }

        public static void Write(BinaryWriter writer, MeshCache mesh)
        {
            writer.Write(mesh.Vertices.Count);

            foreach (var vertex in mesh.Vertices)
                Vertex.Write(writer, vertex);

            writer.Write(mesh.Indices.Count);

            foreach (var index in mesh.Indices)
                writer.Write(index);

            writer.Write((int)mesh.Topology);
        }

        public static MeshCache Read(BinaryReader reader)
        {
            var mesh        = new MeshCache();
            var vertexCount = reader.ReadInt32();

            for (int i = 0; i < vertexCount; i++)
                mesh.Vertices.Add(Vertex.Read(reader));

            var indexCount = reader.ReadInt32();

            for (int i = 0; i < indexCount; i++)
                mesh.Indices.Add(reader.ReadInt32());

            mesh.Topology = (MeshTopology)reader.ReadInt32();

            return mesh;
        }
    }
}
