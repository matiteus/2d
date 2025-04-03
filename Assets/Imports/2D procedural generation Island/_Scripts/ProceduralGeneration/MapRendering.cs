using System;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Responsible for painting the tiles on the Tilemaps based on the GenerationData. This separates Rendering from
/// procedural generation. This way you can reuse this system for a different set of Tiles.
/// </summary>
public class MapRendering : MonoBehaviour
{
    [Header("Tilemaps")]
    [SerializeField] private Tilemap m_baseTilemap;
    [SerializeField] private Tilemap m_treeTilemap;
    [SerializeField] private Tilemap m_fixTilemap;
    [SerializeField] private Tilemap m_grassTilemap;

    [Header("Tiles")]
    [SerializeField] private TileBase m_waterTile;
    [SerializeField] private TileBase m_greenGroundTile;
    [SerializeField] private TileBase m_hillTile;
    [SerializeField] private TileBase m_seaTile;
    [SerializeField] private TileBase m_sandTile;
    [SerializeField] private TileBase m_sandGrass;
    [SerializeField] private TileBase m_hillLevel2Tile;
    [SerializeField] private TileBase m_grassTile;
    [SerializeField] private TileBase m_greenTreeTile;
    [SerializeField] private TileBase m_palmTreeTile;
    [SerializeField] private TileBase m_hillTreeTile;
    [SerializeField] private TileBase m_seaWaterTile;
    [SerializeField] private TileBase m_deepSeaTile;
    [SerializeField] private TileBase m_deepSeaWaterTile;
    [SerializeField] private TileBase m_stairsUP;
    [SerializeField] private TileBase m_stairsDown;
    [SerializeField] private TileBase m_stairsLeft;
    [SerializeField] private TileBase m_stairsRight;

    /// <summary>
    /// Paints the tiles on the Tilemaps based on the GenerationData
    /// </summary>
    /// <param name="generationData"></param>
    public void PaintTiles(GenerationData generationData)
    {
        m_grassTilemap.ClearAllTiles();
        m_fixTilemap.ClearAllTiles();
        m_treeTilemap.ClearAllTiles();
        m_baseTilemap.ClearAllTiles();

        for (int x = 0; x < generationData.MapWidth; x++)
        {
            for (int y = 0; y < generationData.MapHeight; y++)
            {
                m_baseTilemap.SetTile(new Vector3Int(x, y, 0), GetTileFrom(generationData.BaseMapTiles[x, y]));
                if (generationData.TreeTiles[x, y] != TileType.None)
                {
                    m_treeTilemap.SetTile(new Vector3Int(x, y, 0), GetTileFrom(generationData.TreeTiles[x, y]));
                }
                if (generationData.FixTiles[x, y] != TileType.None)
                {
                    m_fixTilemap.SetTile(new Vector3Int(x, y, 0), GetTileFrom(generationData.FixTiles[x, y]));
                }
            }
        }

        foreach (Vector2Int tilePosition in generationData.GrassPositions)
        {
            m_grassTilemap.SetTile(new Vector3Int(tilePosition.x, tilePosition.y, 0), m_grassTile);
        }

        foreach (Vector2Int tilePosition in generationData.FixSeaEdgesPositions)
        {
            m_fixTilemap.SetTile(new Vector3Int(tilePosition.x, tilePosition.y, 0), m_deepSeaWaterTile);
        }

    }

    /// <summary>
    /// We separate the data (TileType) array from Tilemap. This allows us to specify the tiles that we
    /// want to paint on the Tilemap - making this system reusable if you have different tiles in your project.
    /// </summary>
    /// <param name="tileType"></param>
    /// <returns>Returns a TileBase based on the specified TileType</returns>
    /// <exception cref="NotImplementedException"></exception>
    public TileBase GetTileFrom(TileType tileType)
    {
        return tileType switch
        {
            TileType.Water => m_waterTile,
            TileType.Sea => m_seaTile,
            TileType.Ground => m_greenGroundTile,
            TileType.HillLevel1 => m_hillTile,
            TileType.Sand => m_sandTile,
            TileType.SandGrass => m_sandGrass,
            TileType.HillLevel2 => m_hillLevel2Tile,
            TileType.DeepSea => m_deepSeaTile,
            TileType.Grass => m_grassTile,
            TileType.GreenTree => m_greenTreeTile,
            TileType.PalmTree => m_palmTreeTile,
            TileType.StairsDown => m_stairsDown,
            TileType.StairsLeft => m_stairsLeft,
            TileType.StairsRight => m_stairsRight,
            TileType.StairsUp => m_stairsUP,
            TileType.HillTree => m_hillTreeTile,
            TileType.None => null,
            _ => throw new NotImplementedException()
        };
    }

    /// <summary>
    /// Helper method to get the Tilemap Sprite at a specific world position for test purposes
    /// </summary>
    /// <param name="worldPosition">Input position</param>
    /// <param name="generationData">Generation data</param>
    /// <returns></returns>
    public Sprite GetBaseTilemapSpriteAt(Vector3 worldPosition, GenerationData generationData)
    {
        if (m_baseTilemap == null)
        {
            Debug.LogError("Tilemap is null");
            return null;
        }
        worldPosition.z = 0;
        //get cell position on the m_tilemap from mouse position
        Vector3Int coordinate = m_baseTilemap.WorldToCell(worldPosition);
        //add check if the position is within range
        if (coordinate.x >= 0 && coordinate.x < generationData.MapWidth && coordinate.y >= 0 && coordinate.y < generationData.MapHeight)
        {
            if (m_baseTilemap.HasTile(coordinate))
            {
                return m_baseTilemap.GetSprite(coordinate);
            }

        }
        return null;
    }
}
