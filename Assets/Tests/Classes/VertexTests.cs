using System.IO;
using System.Text;
using NUnit.Framework;
using UnityEngine;
using VoxelWorld.Classes;

namespace VoxelWorld.Testing
{
    public class VertexTests
    {
        [Test]
        public void ReadWrite_Test()
        {
            var file  = ".//Assets/Tests/TestData/Vertex";
            var input = new Vertex() { Position = new Vector3(1, 2, 3), Normal = Vector3.forward, Color = Color.red, UV = Vector2.one };

            Vertex output;

            using var writeStream = File.Open(file, FileMode.Create);
            {
                using var writer = new BinaryWriter(writeStream, Encoding.UTF8, false);
                {
                    Vertex.Write(writer, input);
                }
            }

            using var readStream = File.Open(file, FileMode.Open);
            {
                using var reader = new BinaryReader(readStream, Encoding.UTF8, false);
                {
                    output = Vertex.Read(reader);
                }
            }

            Assert.That(output.Position, Is.EqualTo(input.Position), "Position");
            Assert.That(output.Normal,   Is.EqualTo(input.Normal),   "Normal"  );
            Assert.That(output.Color,    Is.EqualTo(input.Color),    "Color"   );
            Assert.That(output.UV,       Is.EqualTo(input.UV),       "UV"      );
        }
    }
}