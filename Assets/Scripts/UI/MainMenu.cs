using UnityEngine;
using UnityEngine.UI;
using VoxelWorld.Classes;

namespace VoxelWorld.Scripts.UI
{
    public class MainMenu : MonoBehaviour
    {
        public RawImage mapDisplay;

        World World { get; set; }

        static Texture2D GenerateWorldImage(World world)
        {
            var texture = new Texture2D(world.Width, world.Length, TextureFormat.RGB24, 0, true)
            {
                filterMode = FilterMode.Point
            };

            for (int y = 0; y < world.Height; y++)
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
            World = WorldGenerator.Generate(100);

            if (mapDisplay != null)
                mapDisplay.texture = GenerateWorldImage(World);
        }
    }
}