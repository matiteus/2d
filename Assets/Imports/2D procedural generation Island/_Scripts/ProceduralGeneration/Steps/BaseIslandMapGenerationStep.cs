using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generates Base map - Water, Ground and hills. We also apply the Island mask here so that the map
/// is surrounded by sea.
/// </summary>
public class BaseIslandMapGenerationStep : GenerationStep
{
    [Header("Base Generation Settings")]
    [SerializeField, Tooltip("Adds base terrain features")]
    private bool m_applyBaseTerrain = true;
    [SerializeField, Tooltip("Adds a circular mask and applies it onto the base terrain")]
    private bool m_applyIslandMask = true;
    [SerializeField, Tooltip("Adds more details to the otherwise circular island mask")]
    private bool m_applyAdditionalIslandMask = true;

    [Header("Terrain Threshold values")]
    [SerializeField, Range(0, 1)]
    private float m_waterThreshold = 0.2f;
    [SerializeField, Range(0, 1)]
    private float m_hillsThreshold = 0.7f;

    [Header("Island Threshold values")]
    [SerializeField, Range(0, 1), Tooltip("Controls the size of the Island map ONLY if we disable the Additional Island Mask")]
    private float m_islandMaskThreshold = 0.5f;
    [SerializeField, Range(0, 1), Tooltip("If Additional Mask is On controls the Island sie - higher value = less land")]
    private float m_islandAdditionalMaskThreshold = 0.09f;
    [SerializeField, Range(0, 1), Tooltip("The higher the value the more 'long edges' / less circular will the map be")]
    private float m_islandSmoothStepFrom = 0f;
    [SerializeField, Range(0, 1), Tooltip("The lower the value the BIGGER and more Circular the island will be")]
    private float m_islandSmoothStepTo = 0.518f;

    [Header("Noise Generation Data")]
    [SerializeField]
    private NoiseDataSO m_noiseData;
    [SerializeField]
    private NoiseDataSO m_islandNoiseData;

    public override void Execute(GenerationData generationData)
    {
        float[,] maskNoise = BaseIslandMask(generationData);
        BaseIslandGeneration(maskNoise, generationData);
    }

    /// <summary>
    /// Generates the base terrain of the map and applies the island mask.
    /// </summary>
    /// <param name="maskNoiseMap"></param>
    /// <param name="generationData"></param>
    private void BaseIslandGeneration(float[,] maskNoiseMap, GenerationData generationData)
    {
        float[,] baseNoiseMap = null;

        if (m_applyBaseTerrain)
        {
            baseNoiseMap
                = NoseGenerationHelper.GeneratePerlinNoiseMap(generationData.MapWidth, generationData.MapHeight, m_noiseData);
        }


        for (int x = 0; x < generationData.MapWidth; x++)
        {
            for (int y = 0; y < generationData.MapHeight; y++)
            {
                float noiseValue = 0f;

                //If Island Mask is enabled applies its value to the noiseValue
                if (maskNoiseMap != null && x >= 0 && x < maskNoiseMap.GetLength(0) && y >= 0 && y < maskNoiseMap.GetLength(1))
                {
                    noiseValue = maskNoiseMap[x, y];
                }

                //If Base Terrain is enabled applies its value to the noiseValue
                if (m_applyBaseTerrain)
                {
                    float tempValue = baseNoiseMap[x, y];

                    if (m_applyIslandMask)
                    {
                        noiseValue *= tempValue;
                    }
                    else
                    {
                        noiseValue = tempValue;
                    }
                }

                //For safety we clamp the value after applying all the masks
                //hillsThreshold is the highest value we can have for this generation step
                noiseValue = Mathf.Clamp(noiseValue, 0, m_hillsThreshold);

                if (noiseValue < m_waterThreshold)
                {
                    generationData.BaseMapTiles[x, y] = TileType.Water;
                }
                else
                {
                    generationData.BaseMapTiles[x, y] = TileType.Ground;
                }

            }
        }

        //Makes the water tiles around the island deep sea. Otherwise they would be the same as inland water tiles.
        foreach (Vector2Int seaTile in GetAllSeaTileConnectedTo(new Vector2Int(0, 0), generationData.BaseMapTiles))
        {
            generationData.BaseMapTiles[seaTile.x, seaTile.y] = TileType.DeepSea;
        }

    }

    /// <summary>
    /// Applies Island circular mask to shape the map as an island. Additional Island mask adds more details to the island
    /// shape so its shape isn't a perfect circle.
    /// </summary>
    /// <param name="generationData"></param>
    /// <returns></returns>
    private float[,] BaseIslandMask(GenerationData generationData)
    {
        float[,] islandMask = null;
        float[,] islandAdditionalMask = null;
        float[,] baseNoiseMap = new float[generationData.MapWidth, generationData.MapHeight];

        //If Island Mask is enabled generates a circular mask
        if (m_applyIslandMask)
        {
            islandMask = NoseGenerationHelper.GenerateCircularMask(generationData.MapWidth, generationData.MapHeight);
        }
        else
        {
            return null;
        }

        //Only if circular mask was generated add details
        if (m_applyAdditionalIslandMask)
        {
            islandAdditionalMask
                = NoseGenerationHelper.GeneratePerlinNoiseMap(generationData.MapWidth, generationData.MapHeight, m_islandNoiseData);
        }
        for (int x = 0; x < generationData.MapWidth; x++)
        {
            for (int y = 0; y < generationData.MapHeight; y++)
            {
                //Apply more details to the circular mask
                if (m_applyAdditionalIslandMask)
                {
                    baseNoiseMap[x, y] = islandAdditionalMask[x, y];
                    if (baseNoiseMap[x, y] > m_islandAdditionalMaskThreshold)
                        baseNoiseMap[x, y] = 1;
                    else
                        baseNoiseMap[x, y] = 0;
                }

                //Use smoothstep to make the island mask more circular - to smoothe the edges added by the previous calculation
                baseNoiseMap[x, y] = Mathf.SmoothStep(m_islandSmoothStepFrom, m_islandSmoothStepTo, islandMask[x, y]);
                if (m_applyAdditionalIslandMask)
                {
                    //Island mask needs to have 0 in the middle and 1 on the edges to delete the edges
                    //so that there is no land at the edges of the map
                    baseNoiseMap[x, y] = islandAdditionalMask[x, y] - baseNoiseMap[x, y];
                    if (baseNoiseMap[x, y] > m_islandAdditionalMaskThreshold)
                        baseNoiseMap[x, y] = 1;
                    else
                        baseNoiseMap[x, y] = 0;


                    //Apply the additional mask to the island mask so that edges of the world are always water
                    float edgeMaskValue = 1 - islandMask[x, y];
                    edgeMaskValue = edgeMaskValue > m_islandAdditionalMaskThreshold ? 1 : 0;
                    baseNoiseMap[x, y] *= edgeMaskValue;
                }
                else
                {
                    if (baseNoiseMap[x, y] < m_islandMaskThreshold)
                        baseNoiseMap[x, y] = 1;
                    else
                        baseNoiseMap[x, y] = 0;

                }

                //Make tiles into water or ground based on the threshold value
                if (baseNoiseMap[x, y] < m_waterThreshold)
                {
                    generationData.BaseMapTiles[x, y] = TileType.Water;
                }
                else
                {
                    generationData.BaseMapTiles[x, y] = TileType.Ground;
                }
            }
        }

        return baseNoiseMap;
    }

    public Vector2Int[] GetAllSeaTileConnectedTo(Vector2Int startPosition, TileType[,] map)
    {
        List<Vector2Int> connectedTiles = new List<Vector2Int>();
        int mapWidth = map.GetLength(0);
        int mapHeight = map.GetLength(1);

        bool[,] visited = new bool[mapWidth, mapHeight];
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        queue.Enqueue(startPosition);
        visited[(int)startPosition.x, (int)startPosition.y] = true;

        // Check adjacent tiles
        Vector2Int[] adjacentTiles = new Vector2Int[]
        {
                new Vector2Int( 1, 0),
                new Vector2Int(- 1, 0),
                new Vector2Int(0, 1),
                new Vector2Int(0, - 1)
        };

        while (queue.Count > 0)
        {
            Vector2Int currentTile = queue.Dequeue();
            connectedTiles.Add(currentTile);

            foreach (Vector2Int offset in adjacentTiles)
            {
                Vector2Int adjacentTile = currentTile + offset;
                int x = adjacentTile.x;
                int y = adjacentTile.y;

                // Check if the adjacent tile is within the map boundaries and is of type Water
                if (x >= 0 && x < mapWidth && y >= 0 && y < mapHeight && map[x, y] == TileType.Water && !visited[x, y])
                {
                    queue.Enqueue(adjacentTile);
                    visited[x, y] = true;
                }
            }
        }

        return connectedTiles.ToArray();
    }
}
