using Unity.VisualScripting;
using UnityEngine;
using VoxelWorld.Classes;

namespace VoxelWorld.Scripts
{
    public class DebugDrawer : MonoBehaviour
    {
        public bool drawBlockTarget = false;

        private static void DrawDebugMesh(MeshCache meshCache, Color color)
        {
            var shader   = Shader.Find("Unlit/Color");
            var material = new Material(shader)
            {
                color = color
            };

            Graphics.DrawMesh(meshCache.ToMesh(), Vector3.zero, Quaternion.identity, material, 0);
        }

        private static void DrawBlockTarget(BlockTarget blockTarget)
        {
            var meshCache = new MeshCache
            {
                topology = MeshTopology.Lines
            };

            Meshing.GenerateCube(meshCache, blockTarget.position, 1);

            DrawDebugMesh(meshCache, Color.red);
        }

        private void Update()
        {
            if (drawBlockTarget && Camera.main != null)
            {
                var target = BlockTarget.GetTarget(Camera.main);

                if (target != null)
                    DrawBlockTarget(target);
            }
        }
    }
}