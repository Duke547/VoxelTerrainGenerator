using System.IO;
using System.Text;
using NUnit.Framework;
using UnityEngine;

namespace VoxelWorld.Testing
{
    public class BinaryReaderWriterExtTests
    {
        [Test]
        public void Vector2_Test()
        {
            var file   = ".//Assets/Tests/TestData/Vector2";
            var input  = new Vector2(1, 2);
            var output = new Vector2();

            using var writeStream = File.Open(file, FileMode.Create);
            {
                using var writer = new BinaryWriter(writeStream, Encoding.UTF8, false);
                {
                    writer.WriteVector2(input);
                }
            }

            using var readStream = File.Open(file, FileMode.Open);
            {
                using var reader = new BinaryReader(readStream, Encoding.UTF8, false);
                {
                    output = reader.ReadVector2();
                }
            }

            Assert.That(output.x, Is.EqualTo(input.x), "X");
            Assert.That(output.y, Is.EqualTo(input.y), "Y");
        }

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

        [Test]
        public void Color_Test()
        {
            var file  = ".//Assets/Tests/TestData/Color";
            var input = Color.blue;
            
            Color output;

            using var writeStream = File.Open(file, FileMode.Create);
            {
                using var writer = new BinaryWriter(writeStream, Encoding.UTF8, false);
                {
                    writer.WriteColor(input);
                }
            }

            using var readStream = File.Open(file, FileMode.Open);
            {
                using var reader = new BinaryReader(readStream, Encoding.UTF8, false);
                {
                    output = reader.ReadColor();
                }
            }

            Assert.That(output.r, Is.EqualTo(input.r), "R");
            Assert.That(output.g, Is.EqualTo(input.g), "G");
            Assert.That(output.b, Is.EqualTo(input.b), "B");
            Assert.That(output.a, Is.EqualTo(input.a), "A");
        }
    }
}