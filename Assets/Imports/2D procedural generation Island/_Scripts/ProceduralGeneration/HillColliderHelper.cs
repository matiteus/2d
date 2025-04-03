using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Helps with giving Hill Edges a correct collider. We need it because when using RuleTiles we will not get the 
/// correct collider if we remove the "interior" of the till. The RuleTile will select a different sprite
/// than it would assign to the "baseTilemap" where we place on the edge and the interior area of the hills.
/// </summary>
public class HillColliderHelper : MonoBehaviour
{
    [SerializeField]
    private List<SpriteTileBasePair> m_hillSpriteTileBasePairs;

    /// <summary>
    /// If we didn't define a sprite we will use a tile that will generate a full tile collider.
    /// </summary>
    [SerializeField]
    private TileBase m_defaultTile;

    public TileBase GetTileForSprite(Sprite hillTileSprite)
    {
        foreach (var pair in m_hillSpriteTileBasePairs)
        {
            if (pair.Sprite == hillTileSprite)
            {
                return pair.Tile;
            }
        }
        return m_defaultTile;
    }
}
/// <summary>
/// Defines the TileBase with a correct Physics Shape for the given Sprite representing the Hill Edge.
/// https://docs.unity3d.com/Manual/CustomPhysicsShape.html
/// </summary>
[Serializable]
public class SpriteTileBasePair
{
    public Sprite Sprite;
    public TileBase Tile;
}