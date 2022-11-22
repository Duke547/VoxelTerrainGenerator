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
            using (new ProfilerMarker($"{nameof(BlockType)}.GetType").Auto())
            {
                if (id == BlockID.Dirt)
                    return Dirt;

                return Air;
            }
        }

        public static BlockType Air  = new("Air",  false);
        public static BlockType Dirt = new("Dirt", true );
    }
}