using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GridPathManager : MonoBehaviour
{

    public static GridPathManager Instance { get; private set; }
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private LineRenderer linePrefab;

    private Tile startTile;
    private List<Tile> currentPath = new List<Tile>();
    private LineRenderer currentLine;

    private string currentColor = "";


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        foreach (Tile tile in GetComponentsInChildren<Tile>())
        {
            // Parse name to extract grid position
            string[] parts = tile.name.Split(',');
            if (parts.Length == 2)
            {
                int x, y;
                if (int.TryParse(parts[0], out x) && int.TryParse(parts[1], out y))
                {
                    tile.GridPosition = new Vector2Int(x, y);

                }
                else
                {
                    Debug.LogError($"Failed to parse grid position for tile {tile.gameObject.name}");
                }
            }
            else
            {
                Debug.LogError($"Tile name format invalid for {tile.gameObject.name}");
            }
        }
    }



    public void StartNewPath(Tile tile)
    {
        
        startTile = tile;
        currentColor = tile.dotColor;
        currentPath.Clear();
        currentPath.Add(startTile);

        currentLine = Instantiate(linePrefab, transform);
        currentColor = startTile.dotColor;
        SetLineColor(currentLine, currentColor);
        Debug.Log("Starting new path with color: " + currentColor);
        currentLine.positionCount = 1;
        currentLine.SetPosition(0, startTile.transform.position);
            
    }

    public void ExtendPath(Tile tile)
    {
        if (!currentPath.Contains(tile))
        {
            Tile lastTile = currentPath[currentPath.Count - 1];
            if (IsAdjacent(lastTile.GridPosition, tile.GridPosition))
            {
                if (tile.IsOccupied)
                {
                    if (tile.IsDot)
                    {
                        if (currentColor != tile.dotColor)
                        {
                            DestroyCurrentPath();
                        }
                        else
                        {
                            currentPath.Add(tile);
                            tile.IsOccupied = true;
                            tile.Color = currentColor;
                            UpdateLineRenderer();
                        }
                    }
                    else
                    {

                        GridManager.Instance.LineDestroyed(tile.Color);
                        DestroyCurrentPath();
                    }
                    
                }
                else
                {
                    currentPath.Add(tile);
                    tile.IsOccupied = true;
                    tile.Color = currentColor;
                    UpdateLineRenderer();
                }
            }
        }
        else
        {
            RemoveLastPathStep(tile);
        }
    }

        public void FinishPath(Tile lastTile)
        {
        if (lastTile)
        {
            if (lastTile.IsDot && lastTile.dotColor == currentColor && lastTile != startTile)
            {
                Debug.Log(lastTile.name);
                
                if (!startTile)
                {
                }
                else
                {
                }
                    UpdateLineRenderer();
                GridManager.Instance.SaveLinePath(currentPath, currentColor,currentLine);
                GridManager.Instance.ColorsConnected(currentColor);
            }
            else
            {
                DestroyCurrentPath();
            }
        }
        else
        {
            DestroyCurrentPath();
        }       

    }

    private void DestroyCurrentPath()
    {
        HandControl.Instance.LineDestroyed();
        foreach (Tile tile in currentPath)
        {
            tile.IsOccupied = false;
            tile.Color = "";
        }
        if (currentLine != null)
        {
            Destroy(currentLine.gameObject);
        }
        currentPath.Clear();
    }


    private bool IsAdjacent(Vector2Int a, Vector2Int b)
    {
        return (Mathf.Abs(a.x - b.x) == 1 && a.y == b.y) || (Mathf.Abs(a.y - b.y) == 1 && a.x == b.x);
    }

    private void UpdateLineRenderer()
    {
        if(!currentLine) currentLine = Instantiate(linePrefab, transform);
        currentLine.positionCount = currentPath.Count;
        for (int i = 0; i < currentPath.Count; i++)
        {
            currentLine.SetPosition(i, currentPath[i].transform.position);
        }
    }

    public void RemoveLastPathStep(Tile tile)
    {

        Tile lastTile = currentPath[currentPath.Count - 2];
        if (lastTile == tile)
        {
            Tile removedTile = currentPath[currentPath.Count - 1];
            removedTile.IsOccupied = false;
            removedTile.Color = "";
            currentPath.Remove(removedTile);
            UpdateLineRenderer();
        }
    }

    public void RestartPath(Tile startTile, Tile removedTile)
    {
        Destroy(currentLine.gameObject);
        StartNewPath(startTile);

    }
    private void SetLineColor(LineRenderer currentLine, string color)
    {
        if (color == "Blue")
        {
            currentLine.startColor = Color.blue;
            currentLine.endColor = Color.blue;
        }
        else if (color == "Green")
        {
            currentLine.startColor = Color.green;
            currentLine.endColor = Color.green;
        }
        else if (color == "Red")
        {
            currentLine.startColor = Color.red;
            currentLine.endColor = Color.red;
        }
        else
        {
            Debug.LogWarning("Unknown color: " + color);
        }
    }
}
