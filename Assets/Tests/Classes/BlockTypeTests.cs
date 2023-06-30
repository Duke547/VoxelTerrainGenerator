using NUnit.Framework;

namespace VoxelWorld.Testing
{
    public class BlockTypeTests
    {
        [Test]
        public void GetBlockType_Test()
        {
            var blockTypeName = "Dirt";
            var blockType     = BlockType.GetBlockType(blockTypeName);

            Assert.That(blockType.Name, Is.EqualTo(blockTypeName));
        }

        [Test]
        public void GetBlockType_InvalidName_Test()
        {
            var blockType = BlockType.GetBlockType("Dirtz");

            Assert.That(blockType, Is.Null);
        }

        [Test]
        public void GetBlockTypeID_Test()
        {
            var index = BlockType.GetBlockTypeID("Dirt");

            Assert.That(index, Is.EqualTo(1));
        }

        [Test]
        public void GetBlockTypeID_InvalidName_Test()
        {
            var blockType = BlockType.GetBlockTypeID("Dirtz");

            Assert.That(blockType, Is.Zero);
        }
    }
}