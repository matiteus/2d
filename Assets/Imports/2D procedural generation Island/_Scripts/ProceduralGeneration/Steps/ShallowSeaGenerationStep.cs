using System.Collections.Generic;
using UnityEngine;

public class ShallowSeaGenerationStep : GenerationStep
{
    [SerializeField]
    private bool m_generateShallowSea = true;

    [SerializeField]
    private int m_shallowSeaWidth = 2;

    [SerializeField, Tooltip("TileTypes to move to when expending the edge line")]
    List<TileType> m_tilesToSearchFor = new List<TileType>
        {
            TileType.Sand,
            TileType.SandGrass,
        };
    /// <summary>
    /// Similar to ShoreLine generation expends the edge of the water and creates shallow sea tiles
    /// where we previously had a deep sea.
    /// </summary>
    /// <param name="generationData"></param>
    public override void Execute(GenerationData generationData)
    {
        if (m_generateShallowSea == false)
            return;

        HashSet<Vector2Int> initialShoreTiles
            = GenerationUtils.GetEdgeTiles(m_tilesToSearchFor, new List<TileType> { TileType.DeepSea }, DirectionsHelper.DirectionOffsets4, generationData.BaseMapTiles);

        HashSet<Vector2Int> shallowSeaPositions = GenerationUtils.ExpandEdgeTiles(initialShoreTiles, m_shallowSeaWidth, new List<TileType> { TileType.DeepSea }, generationData.BaseMapTiles);

        foreach (Vector2Int tilePositon in shallowSeaPositions)
        {
            generationData.BaseMapTiles[tilePositon.x, tilePositon.y] = TileType.Sea;
        }

    }
}
