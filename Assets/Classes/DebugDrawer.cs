using UnityEngine;

namespace VoxelWorld.Classes
{
    public static class DebugDrawer
    {
        static void DrawDebugMesh(MeshCache meshCache, Color color)
        {
            var shader   = Shader.Find("Unlit/Color");
            var material = new Material(shader);

            meshCache.topology = MeshTopology.Lines;
            material .color    = color;

            Graphics.DrawMesh(meshCache.ToMesh(), Vector3.zero, Quaternion.identity, material, 0);
        }

        public static void DrawBlockTarget(BlockTarget blockTarget)
        {
            var meshCache = new MeshCache();

            Meshing.GenerateCube(meshCache, blockTarget.position, 1);

            DrawDebugMesh(meshCache, Color.red);
        }
    }
}