using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Generates tile colliders for Hill Edges and the Deep Sea to block the player from walking on them.
/// </summary>
public class TilesCollidersGenerator : MonoBehaviour
{
    [SerializeField]
    private HillColliderHelper m_hillColliderHelper;

    /// <summary>
    /// We will use TilemapCollider2D to generate colliders. All we need to to add tiles in specific places
    /// on those Tilemaps:
    /// </summary>
    [SerializeField]
    private Tilemap m_colliderHillsTilemap, m_colliderDeepSeaTilemap;

    /// <summary>
    /// To correctly generate Hill Edge colliders we need to ask baseTilemap about its tiles representing the Hill Edges.
    /// </summary>
    [SerializeField]
    private Tilemap m_baseTilemap;

    /// <summary>
    /// To get the correct DeepSee colliders (since we use a RuleTile for those) we need to ask the MapRendering for the
    /// tiles representing the DeepSea and set the same time onto the colliderDeepSeaTilemap.
    /// </summary>
    [SerializeField]
    private MapRendering m_mapRendering;

    /// <summary>
    /// Generate the colliders needed by our Island Map
    /// </summary>
    /// <param name="generationData"></param>
    public void AddColliders(GenerationData generationData)
    {
        if (m_hillColliderHelper == null)
        {
            Debug.LogError("HillColliderHelper is not assigned. Cant add colliders to hills.");
            return;
        }

        m_colliderDeepSeaTilemap.ClearAllTiles();
        m_colliderHillsTilemap.ClearAllTiles();


        for (int x = 0; x < generationData.MapWidth; x++)
        {
            for (int y = 0; y < generationData.MapHeight; y++)
            {
                AddHillEdgeAndDeepSeaColliderTiles(generationData, x, y);
            }
        }
    }

    /// <summary>
    /// Adds the Tiles that represent the Hill Edges and the DeepSea to the colliderTilemaps.
    /// </summary>
    /// <param name="generationData"></param>
    /// <param name="x">Tile position X</param>
    /// <param name="y">Tile position Y</param>
    private void AddHillEdgeAndDeepSeaColliderTiles(GenerationData generationData, int x, int y)
    {
        if (generationData.BaseMapTiles[x, y] == TileType.DeepSea)
        {
            m_colliderDeepSeaTilemap.SetTile(new Vector3Int(x, y, 0), m_mapRendering.GetTileFrom(generationData.BaseMapTiles[x, y]));
        }
        else if (generationData.BaseMapTiles[x, y] == TileType.HillLevel1 || generationData.BaseMapTiles[x, y] == TileType.HillLevel2)
        {
            Vector2Int position = new(x, y);
            if ((generationData.HillLevel1Edge.Contains(position) || generationData.HillLevel2Edge.Contains(position)) && generationData.HillStairPositions.Contains(position) == false)
            {
                SetHillEdgeTileWithCorrectRotationMirroring(x, y);
            }

        }
    }

    /// <summary>
    /// Because RuleTile rules can mirror / rotate Tiles we need to copy the Matrix4x4 
    /// from the baseTilemap to the collider tilemap to make the collider shape math the visuals
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    private void SetHillEdgeTileWithCorrectRotationMirroring(int x, int y)
    {
        Vector3Int tilePosition = new(x, y, 0);
        Sprite hillSprite = m_baseTilemap.GetSprite(tilePosition);
        Matrix4x4 matrix = m_baseTilemap.GetTransformMatrix(tilePosition);
        TileBase hillColliderTile = m_hillColliderHelper.GetTileForSprite(hillSprite);
        m_colliderHillsTilemap.SetTile(tilePosition, hillColliderTile);
        m_colliderHillsTilemap.SetTransformMatrix(tilePosition, matrix);
    }
}
