using UnityEngine;

public class GrassGenerationStep : GenerationStep
{
    [SerializeField]
    private bool m_applyGrass = true;

    [SerializeField, Range(0, 1), Tooltip("The higher the threshold the less grass")]
    private float m_grassThreshold = 0.5f;

    [SerializeField] private NoiseDataSO m_grassNoiseData;

    /// <summary>
    /// We use perlin noise to add grass to our map.
    /// </summary>
    /// <param name="generationData"></param>
    public override void Execute(GenerationData generationData)
    {
        if (m_applyGrass)
        {

            float[,] grassMap = NoseGenerationHelper.GeneratePerlinNoiseMap(generationData.MapWidth, generationData.MapHeight, m_grassNoiseData);

            for (int x = 0; x < generationData.MapWidth; x++)
            {
                for (int y = 0; y < generationData.MapHeight; y++)
                {
                    bool isHillEdgeTile = generationData.HillLevel1Edge.Contains(new(x, y))
                        || generationData.HillLevel2Edge.Contains(new(x, y));

                    //Adds grass on the ground and on the level 1 hills
                    if (grassMap[x, y] > (1 - m_grassThreshold) && (generationData.BaseMapTiles[x, y] == TileType.Ground
                        || generationData.BaseMapTiles[x, y] == TileType.HillLevel1) && isHillEdgeTile == false)
                    {
                        generationData.GrassPositions.Add(new(x, y));
                    }

                }
            }
        }
    }
}
