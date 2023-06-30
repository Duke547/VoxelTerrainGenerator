using System.IO;
using UnityEngine;
using VoxelWorld.Extentions;

namespace VoxelWorld
{
    public record Vertex
    {
        public Vector3 Position { get; set; }

        public Vector3 Normal { get; set; }

        public Vector2 UV { get; set; }

        public Color Color { get; set; } = Color.white;

        public override string ToString() => Position.ToString();

        public static void Write(BinaryWriter writer, Vertex vertex)
        {
            writer.WriteVector3(vertex.Position);
            writer.WriteVector3(vertex.Normal  );
            writer.WriteColor  (vertex.Color   );
            writer.WriteVector2(vertex.UV      );
        }

        public static Vertex Read(BinaryReader reader)
        {
            var position = reader.ReadVector3();
            var normal   = reader.ReadVector3();
            var color    = reader.ReadColor  ();
            var uv       = reader.ReadVector2();

            return new() { Position = position, Normal = normal, Color = color, UV = uv };
        }
    }
}
