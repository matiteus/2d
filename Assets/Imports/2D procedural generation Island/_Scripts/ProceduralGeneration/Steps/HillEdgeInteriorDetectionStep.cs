using System.Collections.Generic;
using UnityEngine;

public class HillEdgeInteriorDetectionStep : GenerationStep
{
    HashSet<Vector2Int> m_hill1EdgeTiles = new(), m_hill2EdgeTiles = new(),
        m_hill1InteriorTiles = new(), m_hill2InteriorTiles = new();
    public override void Execute(GenerationData generationData)
    {

        List<TileType> hill1NonEdgeTiles = new() { TileType.HillLevel1, TileType.HillLevel2 };
        List<TileType> hill2NonEdgeTiles = new() { TileType.HillLevel2 };

        for (int x = 0; x < generationData.MapWidth; x++)
        {
            for (int y = 0; y < generationData.MapHeight; y++)
            {
                if (generationData.BaseMapTiles[x, y] == TileType.HillLevel1)
                {
                    if (GenerationUtils.IsEdgeToTiles(x, y, hill1NonEdgeTiles, DirectionsHelper.DirectionOffsets8, generationData.BaseMapTiles))
                    {
                        generationData.HillLevel1Edge.Add(new(x, y));
                    }
                    else
                    {
                        generationData.HillLevel1Interior.Add(new(x, y));
                    }
                }
                if (generationData.BaseMapTiles[x, y] == TileType.HillLevel2)
                {
                    if (GenerationUtils.IsEdgeToTiles(x, y, hill2NonEdgeTiles, DirectionsHelper.DirectionOffsets8, generationData.BaseMapTiles))
                    {
                        generationData.HillLevel2Edge.Add(new(x, y));
                    }
                    else
                    {
                        generationData.HillLevel2Interior.Add(new(x, y));
                    }
                }
            }
        }

        generationData.HillLevel1Interior.ExceptWith(generationData.HillLevel2Interior);
        generationData.HillLevel1Interior.ExceptWith(generationData.HillLevel2Edge);

        m_hill1EdgeTiles = generationData.HillLevel1Edge;
        m_hill2EdgeTiles = generationData.HillLevel2Edge;
        m_hill1InteriorTiles = generationData.HillLevel1Interior;
        m_hill2InteriorTiles = generationData.HillLevel2Interior;

    }

    private void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
        {
            foreach (var pos in m_hill2InteriorTiles)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawCube(new Vector3(pos.x + 0.5f, pos.y + 0.5f, 0), Vector3.one);
            }
            foreach (var pos in m_hill1InteriorTiles)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawCube(new Vector3(pos.x + 0.5f, pos.y + 0.5f, 0), Vector3.one);
            }
            foreach (var pos in m_hill2EdgeTiles)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawCube(new Vector3(pos.x + 0.5f, pos.y + 0.5f, 0), Vector3.one);
            }
            foreach (var pos in m_hill1EdgeTiles)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawCube(new Vector3(pos.x + 0.5f, pos.y + 0.5f, 0), Vector3.one);
            }
        }
    }
}
