using System.Collections.Generic;
using UnityEngine;

public static class GenerationUtils
{

    /// <summary>
    /// Creates the Edge line by finding all the tiles of typesToSearchFor and 
    /// find the ones that are adjacent to typesAdjacentTo
    /// </summary>
    /// <param name="typesToSearchFor">TileTypes to examine</param>
    /// <param name="typesAdjacentTo">Neighbor TileType that defines an Edge</param>
    /// <param name="directions">Should we check the neighbor in 4 or 8 directions</param>
    /// <param name="generationData"></param>
    /// <returns></returns>
    public static HashSet<Vector2Int> GetEdgeTiles(List<TileType> typesToSearchFor, List<TileType> typesAdjacentTo, List<Vector2Int> directions, TileType[,] mapTiles)
    {
        HashSet<Vector2Int> shorelineTiles = new HashSet<Vector2Int>();

        for (int x = 0; x < mapTiles.GetLength(0); x++)
        {
            for (int y = 0; y < mapTiles.GetLength(1); y++)
            {
                if (typesToSearchFor.Contains(mapTiles[x, y])
                    && IsAdjacentToTiles(x, y, typesAdjacentTo, directions, mapTiles))
                {
                    shorelineTiles.Add(new Vector2Int(x, y));
                }
            }
        }

        return shorelineTiles;
    }

    /// <summary>
    /// Checks if tile is adjacent to a tileType specified in the tileTypes
    /// </summary>
    /// <param name="x">Map position X</param>
    /// <param name="y">Map position Y</param>
    /// <param name="tileTypes">Correct Neighbor TileTypes</param>
    /// <param name="directions">In what directions to check</param>
    /// <param name="mapTiles">Array to check</param>
    /// <returns></returns>
    public static bool IsAdjacentToTiles(int x, int y, List<TileType> tileTypes,
        List<Vector2Int> directions, TileType[,] mapTiles)
    {
        foreach (Vector2Int direction in directions)
        {
            int nx = x + direction.x;
            int ny = y + direction.y;

            if (nx >= 0 && nx < mapTiles.GetLength(0) && ny >= 0 && ny < mapTiles.GetLength(1))
            {
                if (tileTypes.Contains(mapTiles[nx, ny]))
                {
                    return true;
                }
            }
        }

        return false;
    }


    /// <summary>
    /// Expends the existing tiles edge in the direction of "validTileTypes" 
    /// by a specified "layers" number.
    /// </summary>
    /// <param name="initialShoreTiles">Tiles representing the initial edge</param>
    /// <param name="width">Desired size of the expanded edge</param>
    /// <param name="validTileTypes">TileTypes that can be converted to shoreline</param>
    /// <param name="generationData"></param>
    /// <returns></returns>
    public static HashSet<Vector2Int> ExpandEdgeTiles(HashSet<Vector2Int> initialShoreTiles, int width, List<TileType> validTileTypes, TileType[,] mapTiles)
    {
        HashSet<Vector2Int> expandedEdgeTiles = new HashSet<Vector2Int>();
        HashSet<Vector2Int> currentLayerTiles = new HashSet<Vector2Int>(initialShoreTiles);

        for (int i = 0; i < width; i++)
        {
            HashSet<Vector2Int> nextLayerTiles = new HashSet<Vector2Int>();

            foreach (Vector2Int tile in currentLayerTiles)
            {

                foreach (Vector2Int direction in DirectionsHelper.DirectionOffsets8)
                {
                    Vector2Int adjacentTile = tile + direction;

                    if (adjacentTile.x >= 0 && adjacentTile.x < mapTiles.GetLength(0) && adjacentTile.y >= 0 && adjacentTile.y < mapTiles.GetLength(1))
                    {
                        if (validTileTypes.Contains(mapTiles[adjacentTile.x, adjacentTile.y]))
                        {
                            if (!expandedEdgeTiles.Contains(adjacentTile))
                            {
                                nextLayerTiles.Add(adjacentTile);
                                expandedEdgeTiles.Add(adjacentTile);
                            }
                        }
                    }
                }
            }

            currentLayerTiles = nextLayerTiles;
        }

        return expandedEdgeTiles;
    }

    /// <summary>
    /// Checks if tile is edge (near a tile NOT specified in the tileTypes)
    /// </summary>
    /// <param name="x">Map position X</param>
    /// <param name="y">Map position Y</param>
    /// <param name="nonEdgeTileTypes">Correct Neighbor TileTypes</param>
    /// <param name="directions">In what directions to check</param>
    /// <param name="mapTiles">Array to check</param>
    /// <returns></returns>
    public static bool IsEdgeToTiles(int x, int y, List<TileType> nonEdgeTileTypes,
        List<Vector2Int> directions, TileType[,] mapTiles)
    {
        foreach (Vector2Int direction in directions)
        {
            int nx = x + direction.x;
            int ny = y + direction.y;

            if (nx >= 0 && nx < mapTiles.GetLength(0) && ny >= 0 && ny < mapTiles.GetLength(1))
            {
                if (nonEdgeTileTypes.Contains(mapTiles[nx, ny]) == false)
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Simple A* pathfinding implementation
    /// </summary>
    /// <param name="startPosition"></param>
    /// <param name="destinationPosition"></param>
    /// <param name="grid">Array of TileTypes</param>
    /// <param name="obstacleTileTypes">TileTypes that serves as obstacles</param>
    /// <returns></returns>
    public static List<Vector2Int> FindPathUsingAstar(Vector2Int startPosition, Vector2Int destinationPosition, TileType[,] grid, List<TileType> obstacleTileTypes)
    {
        List<Vector2Int> openList = new List<Vector2Int> { startPosition };
        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        Dictionary<Vector2Int, int> costSoFar = new Dictionary<Vector2Int, int>();

        cameFrom[startPosition] = startPosition;
        costSoFar[startPosition] = 0;

        while (openList.Count > 0)
        {
            Vector2Int current = openList[0];

            if (current == destinationPosition)
            {
                return RetracePath(cameFrom, startPosition, destinationPosition);
            }

            openList.RemoveAt(0);

            foreach (Vector2Int neighbor in GetNeighbors(current, grid, obstacleTileTypes))
            {
                int newCost = costSoFar[current] + 1; // Assume cost between neighbors is 1
                if (!costSoFar.ContainsKey(neighbor) || newCost < costSoFar[neighbor])
                {
                    costSoFar[neighbor] = newCost;
                    openList.Add(neighbor);
                    cameFrom[neighbor] = current;
                }
            }
        }

        return new(); // Path not found
    }

    private static List<Vector2Int> GetNeighbors(Vector2Int position, TileType[,] grid, List<TileType> obstacleTileTypes)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        foreach (Vector2Int direction in directions)
        {
            Vector2Int neighborPos = position + direction;
            if (IsInBounds(neighborPos, grid) && !obstacleTileTypes.Contains(grid[neighborPos.x, neighborPos.y]))
            {
                neighbors.Add(neighborPos);
            }
        }

        return neighbors;
    }

    private static bool IsInBounds(Vector2Int position, TileType[,] grid)
    {
        return position.x >= 0 && position.x < grid.GetLength(0) && position.y >= 0 && position.y < grid.GetLength(1);
    }

    private static List<Vector2Int> RetracePath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int startPosition, Vector2Int endPosition)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Vector2Int current = endPosition;

        while (current != startPosition)
        {
            path.Add(current);
            current = cameFrom[current];
        }

        path.Add(startPosition);
        path.Reverse();
        return path;
    }
}
