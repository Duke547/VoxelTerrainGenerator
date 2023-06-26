using UnityEngine;
using VoxelWorld.Classes;

namespace VoxelWorld.Scripts
{
    [RequireComponent(typeof(TerrainLoader))]
    public class WorldSpawner : MonoBehaviour
    {
        public int size = 250;

        public World World { get; private set; }

        void Start()
        {
            World = WorldGenerator.Generate(size);
            
            var terrainLoader = GetComponent<TerrainLoader>();
            terrainLoader.world = World;
            
            terrainLoader.LoadChunk(World.PlayerSpawn);
        }
    }
}