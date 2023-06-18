using UnityEngine;

namespace VoxelWorld.Classes
{
    public record Vertex
    {
        public Vector3 Position { get; private set; }

        public Vector3 Normal { get; private set; }

        public Vector2 UV { get; private set; }

        public Color Color { get; private set; }

        public override string ToString() => Position.ToString();

        public Vertex(Vector3 position, Vector3 normal, Vector2 uv, Color color)
        {
            Position = position;
            Normal   = normal.normalized;
            UV       = uv;
            Color    = color;
        }

        public Vertex(Vector3 position, Vector3 normal, Vector2 uv) :
            this(position, normal, uv, Color.white) { }
    }
}
