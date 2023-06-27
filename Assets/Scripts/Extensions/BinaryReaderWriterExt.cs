using System.IO;
using UnityEngine;

namespace VoxelWorld
{
    public static class BinaryReaderWriterExt
    {
        public static void WriteVector3(this BinaryWriter writer, Vector3 vector)
        {
            writer.Write(vector.x);
            writer.Write(vector.y);
            writer.Write(vector.z);
        }

        public static Vector3 ReadVector3(this BinaryReader reader)
        {
            var x = reader.ReadSingle();
            var y = reader.ReadSingle();
            var z = reader.ReadSingle();

            return new(x, y, z);
        }
    }
}