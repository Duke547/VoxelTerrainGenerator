using UnityEngine;
using UnityEngine.UI;
using VoxelWorld.Classes;

namespace VoxelWorld.Scripts.UI
{
    public class MainMenu : MonoBehaviour
    {
        public RawImage mapImage;

        World world;

        Texture2D GenerateWorldTexture()
        {
            var texture = new Texture2D(world.Width, world.Length, TextureFormat.RGB24, 0, true)
            {
                filterMode = FilterMode.Point
            };

            for (int y = 0; y < world.Length; y++)
            {
                for (int x = 0; x < world.Width; x++)
                {
                    var blockLoc = Vector3Int.RoundToInt(world.FindSurface(x, y));
                    var color    = new Color(120/255f, 79/255f, 55/255f) * blockLoc.y / world.Height;

                    texture.SetPixel(x, y, color);
                }
            }
            
            texture.Apply();

            return texture;
        }

        public void GenerateWorld()
        {
            world = WorldGenerator.Generate(1000);

            if (mapImage != null)
            {
                mapImage.texture = GenerateWorldTexture();
                mapImage.color   = Color.white;
            }
        }
    }
}