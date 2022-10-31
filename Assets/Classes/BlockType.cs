using Unity.Profiling;

namespace VoxelWorld.Classes
{
    public record BlockType
    {
        public string Name { get; }
    
        public bool IsSolid { get; }
    
        public BlockType(string name, bool isSolid)
        {
            Name    = name;
            IsSolid = isSolid;
        }

        public static BlockType GetType(BlockID id)
        {
            using (new ProfilerMarker("TerrainLoader.GenerateBlockFace").Auto())
            {
                if (id == BlockID.Dirt)
                    return new("Dirt", true);

                return new("Air", false);
            }
        }
    }
}