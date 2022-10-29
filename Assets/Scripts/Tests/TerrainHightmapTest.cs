using UnityEngine;
using UnityEngine.UI;

public class TerrainHightmapTest : MonoBehaviour
{
    Texture2D GenerateTexture(int width, int length)
    {
        var heightData = TerrainHeightGenerator.Generate(width, length);
        var texture    = new Texture2D(width, length);

        for (int y = 0; y < length; y++)
        {
            for (int x = 0; x < width; x++)
            {
                texture.SetPixel(x, y, new(heightData[x, y], heightData[x, y], heightData[x, y]));
            }
        }

        texture.Apply();

        return texture;
    }

    void GenerateHeightMap(int width, int length)
    {
        var heightmap = GenerateTexture(width, length);
        var image     = FindObjectOfType<RawImage>();

        image.texture = heightmap;
    }

    void Start()
    {
        GenerateHeightMap(1000, 1000);
    }
}
