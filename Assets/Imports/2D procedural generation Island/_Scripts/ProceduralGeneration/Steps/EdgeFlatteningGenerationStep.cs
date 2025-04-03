using System.Collections.Generic;
using UnityEngine;

public class EdgeFlatteningGenerationStep : GenerationStep
{
    [SerializeField]
    private bool m_applyStep = true;

    [SerializeField, Tooltip("If tile has more neighbors of a specified type than the limit it will be removed")]
    private int m_neighborsLimit = 1;

    /// <summary>
    /// TileType to flatten
    /// </summary>
    [SerializeField]
    private TileType m_tileToFlatten;
    /// <summary>
    /// Neighbors TileTypes to detect
    /// </summary>
    [SerializeField]
    private List<TileType> m_neighborTilesToDetect = new();
    /// <summary>
    /// Tile to set in place of removed tile
    /// </summary>
    [SerializeField]
    private TileType m_tileToSet;

    /// <summary>
    /// Loops through all the tiles finding the one to flatten. Next checks if it has more neighbors 
    /// of a specified type than the limit. If so swaps the tile with the one specified in m_tileToSet.
    /// </summary>
    /// <param name="generationData"></param>
    public override void Execute(GenerationData generationData)
    {
        if (m_applyStep == false)
            return;
        HashSet<Vector2Int> m_positionsToFix = new();
        for (int x = 0; x < generationData.MapWidth; x++)
        {
            for (int y = 0; y < generationData.MapHeight; y++)
            {
                if (generationData.BaseMapTiles[x, y] == m_tileToFlatten)
                {
                    //if more then neighborLimit neighbors in 8 directions are neighborTiles add to m_positionsToFix
                    int count = 0;
                    foreach (Vector2Int direction in DirectionsHelper.DirectionOffsets8)
                    {
                        int nx = x + direction.x;
                        int ny = y + direction.y;
                        if (nx >= 0 && nx < generationData.MapWidth && ny >= 0 && ny < generationData.MapHeight)
                        {
                            if (m_neighborTilesToDetect.Contains(generationData.BaseMapTiles[nx, ny]))
                            {
                                count++;
                            }
                        }
                    }
                    if (count >= m_neighborsLimit)
                    {
                        m_positionsToFix.Add(new(x, y));
                    }


                }
            }
        }
        foreach (var pos in m_positionsToFix)
        {
            generationData.BaseMapTiles[pos.x, pos.y] = m_tileToSet;
        }
    }
}
