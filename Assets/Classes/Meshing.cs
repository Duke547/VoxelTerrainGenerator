using System;
using UnityEngine;

using static UnityEngine.Vector3;

namespace VoxelWorld.Classes
{
    public static class Meshing
    {
        public static void GenerateCube(MeshCache mesh, Vector3 center, float size)
        {
            var radius = size / 2;
            var v1     = center + (left + back + down) * radius;
            var v2     = v1 + right   * size;
            var v3     = v2 + forward * size;
            var v4     = v3 + left    * size;
            var v5     = v1 + up      * size;
            var v6     = v5 + right   * size;
            var v7     = v6 + forward * size;
            var v8     = v7 + left    * size;

            mesh.Vertices.AddRange(new[] { v1, v2, v3, v4, v5, v6, v7, v8 });
            mesh.Indices .AddRange(new[] { 0, 1, 1, 2, 2, 3, 3, 0, 4, 5, 5, 6, 6, 7, 7, 4, 0, 4, 1, 5, 2, 6, 3, 7});
        }
    }
}