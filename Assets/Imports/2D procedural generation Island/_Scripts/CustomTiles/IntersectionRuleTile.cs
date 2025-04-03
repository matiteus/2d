using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class IntersectionRuleTile : RuleTile
{
    [SerializeField] private List<TileBase> m_compatibleTiles; // Assign your sea tile here through the Inspector

    public override bool RuleMatch(int neighbor, TileBase other)
    {
        if (neighbor == TilingRule.Neighbor.This && (other == this || m_compatibleTiles.Contains(other)))
        {
            return true;
        }
        else if (neighbor == TilingRule.Neighbor.NotThis && other != this && m_compatibleTiles.Contains(other) == false)
        {
            return true;
        }
        return false;
    }
}
