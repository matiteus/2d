using UnityEngine;

public class FixHillEdgePlacementStep : GenerationStep
{
    [SerializeField]
    private bool m_applyFix = true;

    /// <summary>
    /// We loop through the hill edge tiles and remove the grass and trees from them.
    /// </summary>
    /// <param name="generationData"></param>
    public override void Execute(GenerationData generationData)
    {
        if (m_applyFix == false)
            return;
        foreach (var pos in generationData.HillLevel1Edge)
        {
            generationData.GrassPositions.Remove(pos);
            generationData.TreeTiles[pos.x, pos.y] = TileType.None;
        }
        foreach (var pos in generationData.HillLevel2Edge)
        {
            generationData.GrassPositions.Remove(pos);
            generationData.TreeTiles[pos.x, pos.y] = TileType.None;
        }
    }


}
