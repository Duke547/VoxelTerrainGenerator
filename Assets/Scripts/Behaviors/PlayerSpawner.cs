using UnityEngine;

namespace VoxelWorld.Scripts
{
    [RequireComponent(typeof(WorldSpawner))]
    public class PlayerSpawner : MonoBehaviour
    {
        [Min(15)]
        public int MaxFPS = 300;

        public bool playerSpawned { get; private set; } = false;

        private static void SpawnPlayer(Vector3 position)
        {
            var playerPrefab  = Resources.Load<GameObject>("Prefabs/Player");

            Instantiate(playerPrefab, position, new());
        }

        private void Update()
        {
            Application.targetFrameRate = MaxFPS;

            if (!playerSpawned)
            {
                var world = GetComponent<WorldSpawner>().world;

                if (world != null)
                {
                    SpawnPlayer(world.PlayerSpawn);
                    playerSpawned = true;
                }
            }
        }
    }
}