using Unity.Profiling;
using UnityEngine;

namespace VoxelWorld.Scripts
{
    [RequireComponent(typeof(WorldSpawner))]
    public class PlayerSpawner : MonoBehaviour
    {
        public bool playerSpawned { get; private set; } = false;

        private static void SpawnPlayer(Vector3 position)
        {
            using (new ProfilerMarker($"{nameof(WorldSpawner)}.{nameof(SpawnPlayer)}").Auto())
            {
                var playerPrefab  = Resources.Load<GameObject>("Prefabs/Player");

                Instantiate(playerPrefab, position, new());
            }
        }

        private void Update()
        {
            if (!playerSpawned)
            {
                var world = GetComponent<WorldSpawner>().World;

                if (world != null)
                {
                    SpawnPlayer(world.PlayerSpawn);
                    playerSpawned = true;
                }
            }
        }
    }
}