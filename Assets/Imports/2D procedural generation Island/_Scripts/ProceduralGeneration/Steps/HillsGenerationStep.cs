using UnityEngine;

public class HillsGenerationStep : GenerationStep
{
    [Header("Base Generation Settings")]
    [SerializeField, Tooltip("Enable Disable hill")]
    private bool m_applyHills = true;

    /// <summary>
    /// The bigger the value the more hills we will get
    /// </summary>
    [SerializeField, Range(0, 1)] private float m_hillsLevel1AmountModifier = 0.732f;
    [SerializeField, Range(0, 1)] private float m_hillsLevel2AmountModifier = 0.56f;

    /// <summary>
    /// Circular mask to ensure that hills appears near the center of the island rather than scattered around.
    /// </summary>
    [SerializeField, Range(0, 1)] private float m_hillsCircularMaskModifier = 0.48f;

    [SerializeField]
    private NoiseDataSO m_hillsNoiseData;



    public override void Execute(GenerationData generationData)
    {
        float[,] hillsData = null;
        float[,] hillsCircularMask = null;

        if (m_applyHills)
        {
            hillsData = NoseGenerationHelper
                .GeneratePerlinNoiseMap(generationData.MapWidth, generationData.MapHeight, m_hillsNoiseData);
            hillsCircularMask = NoseGenerationHelper
                .GenerateCircularMask(generationData.MapWidth, generationData.MapHeight, m_hillsCircularMaskModifier);

            for (int x = 0; x < generationData.MapWidth; x++)
            {
                for (int y = 0; y < generationData.MapHeight; y++)
                {
                    //We use One Minus to invert the mask
                    float tempHill = hillsData[x, y] * (1 - hillsCircularMask[x, y]);
                    //We uwe One Minus so that the bigger the value the more hills we will get
                    if (tempHill > (1 - m_hillsLevel1AmountModifier))
                    {
                        generationData.BaseMapTiles[x, y] = TileType.HillLevel1;
                    }
                    if (tempHill > (1 - m_hillsLevel2AmountModifier))
                    {
                        generationData.BaseMapTiles[x, y] = TileType.HillLevel2;
                    }
                }
            }
        }

    }

}
