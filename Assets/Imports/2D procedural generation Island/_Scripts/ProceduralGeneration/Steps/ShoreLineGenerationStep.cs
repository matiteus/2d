using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Expands the shoreline of the map by a specified width.
/// </summary>
public class ShoreLineGenerationStep : GenerationStep
{
    [SerializeField]
    private bool m_createShoreline = true;

    [SerializeField]
    private int m_shoreWidth = 3;

    [SerializeField, Tooltip("TileTypes to move to when expending the edge line")]
    private List<TileType> m_tilesToSearchFor = new List<TileType>
        {
            TileType.Ground,
            TileType.HillLevel1,
            TileType.Water
        };

    private HashSet<Vector2Int> m_gizmoShoreline = new();

    /// <summary>
    /// Adds sand tiles near the water to ensure that our Sea tiles (that uses RuleTile) works correctly.
    /// Adds SandGrass "intersection" tiles to make the transition between the sand and the grass show up
    /// correctly - again based on a RuleTile preset
    /// </summary>
    /// <param name="generationData"></param>
    public override void Execute(GenerationData generationData)
    {
        if (m_createShoreline == false)
            return;
        (HashSet<Vector2Int> shoreTiles, HashSet<Vector2Int> intersectionTiles) = ExpandShoreline(m_shoreWidth, generationData);
        m_gizmoShoreline = shoreTiles;

        foreach (Vector2Int intersectionPosition in intersectionTiles)
        {
            generationData.BaseMapTiles[intersectionPosition.x, intersectionPosition.y] = TileType.SandGrass;
        }
    }

    /// <summary>
    /// Returns a shoreline and the expanded section that intersects with the ground / grass tiles of the island.
    /// We need those 2 to correctly apply RuleTiles when rendering the map.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="generationData"></param>
    /// <returns></returns>
    (HashSet<Vector2Int> shoreTiles, HashSet<Vector2Int> intersectionTiles) ExpandShoreline(int width, GenerationData generationData)
    {
        //TileTypes that we can convert to shore tiles in order to expend the Shoreline

        HashSet<Vector2Int> initialShoreTiles = GenerationUtils.GetEdgeTiles(m_tilesToSearchFor, new List<TileType> { TileType.DeepSea }, DirectionsHelper.DirectionOffsets8, generationData.BaseMapTiles);

        HashSet<Vector2Int> shoreTiles = GenerationUtils.ExpandEdgeTiles(initialShoreTiles, 1, m_tilesToSearchFor, generationData.BaseMapTiles);
        HashSet<Vector2Int> intersectionTiles = GenerationUtils.ExpandEdgeTiles(shoreTiles, width, m_tilesToSearchFor, generationData.BaseMapTiles);

        return (shoreTiles, intersectionTiles);
    }

    private void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
        {
            foreach (var pos in m_gizmoShoreline)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawCube(new Vector3(pos.x + 0.5f, pos.y + 0.5f, 0), Vector3.one);
            }
        }
    }
}
