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
            return Types[(int)id];
        }

        public static readonly BlockType[] Types = new BlockType[]
        {
            new ("Air",  false)
           ,new ("Dirt", true )
        };
    }
}