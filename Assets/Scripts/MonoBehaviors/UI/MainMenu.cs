//using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;

namespace VoxelWorld.MonoBehaviors.UI
{
    public class MainMenu : MonoBehaviour
    {
        public RawImage mapImage;

        static Texture2D GenerateWorldTexture(int size)
        {
            //using (new ProfilerMarker($"{nameof(MainMenu)}.{nameof(GenerateWorldTexture)}").Auto())
            //{
                var surface = WorldGenerator.GenerateSurfaceData(size);
                var texture = new Texture2D(size, size, TextureFormat.RGB24, 0, true)
                {
                    filterMode = FilterMode.Point
                };

                for (int y = 0; y < size; y++)
                {
                    for (int x = 0; x < size; x++)
                    {
                        var color = new Color(120/255f, 79/255f, 55/255f) * surface[x, y];

                        texture.SetPixel(x, y, color);
                    }
                }

                texture.Apply();

                return texture;
           //}
        }

        public void GenerateWorld()
        {
            ////using (new ProfilerMarker($"{nameof(MainMenu)}.{nameof(GenerateWorld)}").Auto())
            //{
                if (mapImage != null)
                {
                    mapImage.texture = GenerateWorldTexture(1000);
                    mapImage.color   = Color.white;
                }
            //}
        }
    }
}