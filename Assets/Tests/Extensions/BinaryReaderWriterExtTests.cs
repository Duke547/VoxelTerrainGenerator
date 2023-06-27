using System.IO;
using System.Text;
using NUnit.Framework;
using UnityEngine;

namespace VoxelWorld.Testing
{
    public class BinaryReaderWriterExtTests
    {
        [Test]
        public void Vector3_Test()
        {
            var file   = ".//Assets/Tests/TestData/Vector3";
            var input  = new Vector3(1, 2, 3);
            var output = new Vector3();
            
            using var writeStream = File.Open(file, FileMode.Create);
            {
                using var writer = new BinaryWriter(writeStream, Encoding.UTF8, false);
                {
                    writer.WriteVector3(input);
                }
            }

            using var readStream = File.Open(file, FileMode.Open);
            {
                using var reader = new BinaryReader(readStream, Encoding.UTF8, false);
                {
                    output = reader.ReadVector3();
                }
            }

            Assert.That(output.x, Is.EqualTo(input.x), "X");
            Assert.That(output.y, Is.EqualTo(input.y), "Y");
            Assert.That(output.z, Is.EqualTo(input.z), "Z");
        }
    }
}