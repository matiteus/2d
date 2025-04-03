using UnityEngine;

public static class NoseGenerationHelper
{
    /// <summary>
    /// Generates Multioctave Perlin Noise Map
    /// </summary>
    /// <returns>Array of noise values</returns>
    public static float[,] GeneratePerlinNoiseMap(int width, int height, NoiseDataSO noiseData)
    {
        return GeneratePerlinNoiseMap(width, height, noiseData.Scale, noiseData.Octaves, noiseData.Persistence, noiseData.Lacunarity, noiseData.Offset);
    }

    /// <summary>
    /// Generates Multioctave Perlin Noise Map
    /// </summary>
    /// <returns>Array of noise values</returns>
    public static float[,] GeneratePerlinNoiseMap(int width, int height, float scale, int octaves, float persistence, float lacunarity, Vector2 offset)
    {
        float[,] noiseMap = new float[width, height];

        if (scale <= 0)
        {
            scale = 0.0001f;
        }

        float maxPossibleHeight = 0;
        float amplitude = 1;

        for (int i = 0; i < octaves; i++)
        {
            maxPossibleHeight += amplitude;
            amplitude *= persistence;
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = (x + offset.x) / scale * frequency;
                    float sampleY = (y + offset.y) / scale * frequency;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY);
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistence;
                    frequency *= lacunarity;
                }

                // Normalize noiseHeight to range [0, 1]
                noiseMap[x, y] = noiseHeight / maxPossibleHeight;
            }
        }

        return noiseMap;
    }

    /// <summary>
    /// Generates a circular mask with values ranging from 0 to 1
    /// </summary>
    /// <param name="width"> Map width</param>
    /// <param name="height"> Map height</param>
    /// <param name="circleRadiusModifier01">Allows to make the circle smaller. 1 = full size. 0 = no circle</param>
    /// <returns></returns>
    public static float[,] GenerateCircularMask(int width, int height, float circleRadiusModifier01 = 1)
    {
        float[,] mask = new float[width, height];
        float radius = Mathf.Min(width, height) / 2; // Radius is half the map's width or height (whichever is smaller)

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float distance =
                    Vector2.Distance(new(x, y), new(width / 2f, height / 2f)) / circleRadiusModifier01;
                mask[x, y] = Mathf.Clamp01(distance / radius);
            }
        }

        return mask;
    }
}
