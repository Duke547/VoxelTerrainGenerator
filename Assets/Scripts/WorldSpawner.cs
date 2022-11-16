using Unity.Profiling;
using UnityEngine;
using VoxelWorld.Classes;

namespace VoxelWorld.Scripts
{
    public class WorldSpawner : MonoBehaviour
    {
        public int size = 250;

        public World World { get; private set; }

        void SpawnTerrainLoader()
        {
            var terrainLoader = gameObject.AddComponent<TerrainLoader>();

            terrainLoader.World = World;
            terrainLoader.LoadChunks(World.PlayerSpawn);
        }

        void SpawnPlayer()
        {
            using (new ProfilerMarker($"{nameof(WorldSpawner)}.{nameof(SpawnPlayer)}").Auto())
            {
                var playerPrefab  = Resources.Load<PlayerController>("Prefabs/Player");

                Instantiate(playerPrefab, World.PlayerSpawn, new());
            }
        }

        void Start()
        {
            World = WorldGenerator.Generate(size);

            SpawnTerrainLoader();
            SpawnPlayer();
        }
    }
}