namespace VoxelWorld
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

        public static BlockType[] Types { get; } = new BlockType[]
        {
            new("Air",  false) // 0
           ,new("Dirt", true ) // 1
        };
    }
}