using System;
using System.Linq;

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
            new("Air",  false),
            new("Dirt", true )
        };

        public static BlockType GetBlockType(string name)
            => Types.FirstOrDefault(t => t.Name == name);

        public static int GetBlockTypeID(string name)
        {
            var blockType = Types.FirstOrDefault(t => t.Name == name);

            if (blockType != null)
                return Array.IndexOf(Types, blockType);

            return 0;
        }
    }
}