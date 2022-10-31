using Unity.Profiling;
using UnityEngine;
using VoxelWorld.Classes;

namespace VoxelWorld.Scripts
{
    public abstract class WorldGenerator : MonoBehaviour
    {
        public World World { get; private set; }

        PlayerController SpawnPlayer(Vector3 location)
        {
            using (new ProfilerMarker("PlayerSpawner.SpawnPlayer").Auto())
            {
                var playerPrefab  = Resources.Load<PlayerController>("Prefabs/Player");
                var terrainLoader = FindObjectOfType<TerrainLoader>();

                return Instantiate(playerPrefab, location, new());
            }
        }

        protected abstract World Generate();

        void Start()
        {
            World = Generate();

            var terrainLoader = FindObjectOfType<TerrainLoader>();

            if (terrainLoader != null)
            {
                terrainLoader.World = World;
                terrainLoader.LoadChunks(World.PlayerSpawn);

                SpawnPlayer(World.PlayerSpawn);
            }
        }
    }
}