using UnityEngine;

namespace VoxelWorld.MonoBehaviors
{
    [RequireComponent(typeof(TerrainLoader))]
    public class WorldSpawner : MonoBehaviour
    {
        public int size = 250;

        public World world { get; private set; }

        void Start()
        {
            world = WorldGenerator.Generate(size);
            
            var terrainLoader = GetComponent<TerrainLoader>();
            terrainLoader.world = world;
            
            terrainLoader.LoadChunk(world.PlayerSpawn);
        }
    }
}