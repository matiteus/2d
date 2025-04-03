using System;
using System.Collections.Generic;
using UnityEngine;

public class PathFromHillsGenerationStep : GenerationStep
{

    [SerializeField]
    private bool m_applyStep = true;
    /// <summary>
    /// To place stairs randomply we will use this random generator to randomly select a mapEdge (see m_mapEdges)
    /// </summary>
    private System.Random m_hillRandom;

    private List<List<Vector2Int>> m_hillsLevel1Paths = new();
    private List<List<Vector2Int>> m_hillsLevel2Paths = new();

    /// <summary>
    /// Edge points of our map used as destination points for our paths to ensure that we can go from hills to shore
    /// </summary>
    private List<Vector2Int> m_mapEdges = new();

    /// <summary>
    /// TileTypes that should are treated s obstacles when finding path from top of hills Level 1
    /// </summary>
    private List<TileType> m_hill1PathObstacleTypes = new List<TileType> { TileType.Water, TileType.HillLevel2 };
    /// <summary>
    /// If we remove HillLevel1 tile when generating the path we replace it with TileType at index [0]
    /// If HillLevel2 with TileType at index [1]
    /// </summary>
    private List<TileType> m_hill1PathSubstituteTypes = new() { TileType.Ground };

    private List<TileType> m_hill2PathObstacleTypes = new List<TileType> { TileType.Water };
    private List<TileType> m_hill2PathSubstituteTypes = new() { TileType.Ground, TileType.HillLevel1 };
    public override void Execute(GenerationData generationData)
    {
        if (m_applyStep == false)
            return;

        //We recreate those in case our map size changes
        m_mapEdges = new()
        {
            new(0, 0),
            new(0, generationData.MapHeight - 1),
            new(generationData.MapWidth - 1, 0),
            new(generationData.MapWidth - 1, generationData.MapHeight - 1)
        };

        //To spawn stairs at the same spots for the same seed we initialize our Random generator 
        //using the given seed
        m_hillRandom = new System.Random(generationData.MapGenerationSeed);
        m_hillsLevel1Paths = AddPathOnTopOfHills(generationData.MappedHillsLevel1, new() { generationData.HillLevel1Edge }, m_hill1PathObstacleTypes, m_hill1PathSubstituteTypes, generationData);
        m_hillsLevel2Paths = AddPathOnTopOfHills(generationData.MappedHillsLevel2, new() { generationData.HillLevel1Edge, generationData.HillLevel2Edge }, m_hill2PathObstacleTypes, m_hill2PathSubstituteTypes, generationData);
    }

    /// <summary>
    /// Adds stairs to enable Player to walk on top of hills. We select a tile from each hill area
    /// and use A* to find the shortest path between it and one of the corners of a map. Next we
    /// add stairs tile where our path crosses the Edge of a hill. We also remove trees from the path.
    /// We treat Water tiles as obstacle.
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private List<List<Vector2Int>> AddPathOnTopOfHills(List<HashSet<Vector2Int>> hills,
        List<HashSet<Vector2Int>> hillEdgeTiles, List<TileType> obstacleTileTypes,
        List<TileType> substituteTilesList, GenerationData generationData)
    {
        List<List<Vector2Int>> paths = new();

        foreach (HashSet<Vector2Int> hillPositions in hills)
        {
            Vector2Int destination = m_mapEdges[m_hillRandom.Next(m_mapEdges.Count)];
            Vector2Int? start = null;
            foreach (var pos in hillPositions)
            {
                start = pos;
                break;
            }
            if (start == null)
                throw new Exception("No start position found");
            List<Vector2Int> path = GenerationUtils.FindPathUsingAstar(start.Value, destination, generationData.BaseMapTiles, obstacleTileTypes);
            paths.Add(path);
            bool[] stairsPlaced = new bool[hillEdgeTiles.Count];
            for (int i = 0; i < path.Count; i++)
            {
                Vector2Int position = path[i];
                int index = 0;
                foreach (var hillEdge in hillEdgeTiles)
                {
                    if (hillEdge.Contains(position))
                    {
                        generationData.HillStairPositions.Add(position);
                        if (stairsPlaced[index] == false)
                        {
                            Vector2Int pathDirection = Vector2Int.up;
                            if (path.Count > i + 1)
                                pathDirection = path[i + 1] - position;
                            generationData.FixTiles[position.x, position.y] = GetStairsTileFor(pathDirection);

                            stairsPlaced[index] = true;
                        }
                        else
                        {
                            generationData.BaseMapTiles[position.x, position.y] = substituteTilesList[index];
                        }
                    }
                    index++;
                }

                generationData.TreeTiles[position.x, position.y] = TileType.None;
            }
        }
        return paths;
    }

    public TileType GetStairsTileFor(Vector2Int direction)
    {
        if (direction == Vector2Int.up)
            return TileType.StairsUp;
        else if (direction == Vector2Int.down)
            return TileType.StairsDown;
        else if (direction == Vector2Int.left)
            return TileType.StairsLeft;
        else
            return TileType.StairsRight;
    }

    List<Color> colors = new();
    private void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
        {

            //Show hill paths
            int index = 0;
            foreach (var hillPath in m_hillsLevel1Paths)
            {
                if (colors.Count > index)
                {
                    Gizmos.color = colors[index];
                }
                else
                {
                    colors.Add(UnityEngine.Random.ColorHSV());
                    Gizmos.color = colors[^1];
                }
                index++;
                foreach (var pos in hillPath)
                {
                    Gizmos.DrawCube(new Vector3(pos.x + 0.5f, pos.y + 0.5f, 0), Vector3.one);
                }
            }
            foreach (var hillPath in m_hillsLevel2Paths)
            {
                if (colors.Count > index)
                {
                    Gizmos.color = colors[index];
                }
                else
                {
                    colors.Add(UnityEngine.Random.ColorHSV());
                    Gizmos.color = colors[^1];
                }
                index++;
                foreach (var pos in hillPath)
                {
                    Gizmos.DrawCube(new Vector3(pos.x + 0.5f, pos.y + 0.5f, 0), Vector3.one);
                }
            }
        }
    }
}
