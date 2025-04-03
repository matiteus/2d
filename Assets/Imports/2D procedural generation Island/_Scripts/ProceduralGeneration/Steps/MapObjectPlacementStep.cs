using System.Collections.Generic;
using UnityEngine;

public class MapObjectPlacementStep : GenerationStep
{
    [SerializeField]
    private List<TileType> m_tileTypesOpenForPlacement = new();
    HashSet<Vector2Int> m_possiblePlacementPositions = new();

    /// <summary>
    /// Loops throught the data that we have about our island map and selects tiles where we can place objects on.
    /// </summary>
    /// <param name="generationData"></param>
    public override void Execute(GenerationData generationData)
    {
        generationData.PossiblePlacementPositions = new();
        HashSet<Vector2Int> treePositions = new();
        for (int x = 0; x < generationData.MapWidth; x++)
        {
            for (int y = 0; y < generationData.MapHeight; y++)
            {
                if (m_tileTypesOpenForPlacement.Contains(generationData.BaseMapTiles[x, y]))
                {
                    generationData.PossiblePlacementPositions.Add(new(x, y));

                }
                if (generationData.TreeTiles[x, y] != TileType.None)
                {
                    treePositions.Add(new(x, y));
                }
            }
        }

        generationData.PossiblePlacementPositions.UnionWith(generationData.HillLevel1Interior);
        generationData.PossiblePlacementPositions.UnionWith(generationData.HillLevel2Interior);
        generationData.PossiblePlacementPositions.ExceptWith(treePositions);

        m_possiblePlacementPositions = generationData.PossiblePlacementPositions;
    }

    private void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
        {
            foreach (var pos in m_possiblePlacementPositions)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawCube(new Vector3(pos.x + 0.5f, pos.y + 0.5f, 0), Vector3.one);
            }
        }
    }
}
