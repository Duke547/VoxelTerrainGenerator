using VoxelWorld.Classes;

namespace VoxelWorld.Scripts
{
    public class BlockTestWorldGenerator : WorldGenerator
    {
        protected override World Generate()
        {
            var world = new World(3);
            var blocks  = world.Blocks;

            blocks[0, 0, 0] = BlockID.Dirt;
            blocks[1, 0, 0] = BlockID.Dirt;
            blocks[2, 0, 0] = BlockID.Dirt;
            blocks[0, 0, 1] = BlockID.Dirt;
            blocks[1, 0, 1] = BlockID.Dirt;
            blocks[2, 0, 1] = BlockID.Dirt;
            blocks[0, 0, 2] = BlockID.Dirt;
            blocks[1, 0, 2] = BlockID.Dirt;
            blocks[2, 0, 2] = BlockID.Dirt;
            blocks[1, 1, 1] = BlockID.Dirt;

            return world;
        }
    }
}