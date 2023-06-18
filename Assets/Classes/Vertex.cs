using UnityEngine;

namespace VoxelWorld.Classes
{
    public record Vertex
    {
        public Vector3 Position { get; set; }

        public Vector3 Normal { get; set; }

        public Vector2 UV { get; set; }

        public Color Color { get; set; } = Color.white;

        public override string ToString() => Position.ToString();
    }
}
