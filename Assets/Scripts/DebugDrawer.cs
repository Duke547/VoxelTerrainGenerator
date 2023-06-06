using Unity.VisualScripting;
using UnityEngine;
using VoxelWorld.Classes;

namespace VoxelWorld.Scripts
{
    public class DebugDrawer : MonoBehaviour
    {

        private PlayerController _player = null;

        private PlayerController player
        {
            get
            {
                if (_player is null || _player.IsDestroyed())
                    _player = FindObjectOfType<PlayerController>();

                return _player;
            }
        }

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
            if (drawBlockTarget && player is not null)
                DrawBlockTarget(player.target);
        }
    }
}