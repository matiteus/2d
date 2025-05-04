using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GridPathManager : MonoBehaviour
{

    public static GridPathManager Instance { get; private set; }
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private LineRenderer linePrefab;
    private Dictionary<Vector2Int, Tile> gridTiles = new Dictionary<Vector2Int, Tile>();

    private Tile startTile;
    private Tile endTile;
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

                    if (!gridTiles.ContainsKey(tile.GridPosition))
                    {
                        gridTiles.Add(tile.GridPosition, tile);
                    }
                    else
                    {
                        Debug.LogWarning($"Duplicate grid position detected at {tile.GridPosition} for tile {tile.gameObject.name}");
                    }
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



    public void StartNewPath(Tile startTile)
    {
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Tile tile = GetTileUnderPosition(mouseWorldPos);

        if (tile != null && tile.IsDot)
        {
            startTile = tile;
            currentColor = tile.DotColor;
            currentPath.Clear();
            currentPath.Add(startTile);

            currentLine = Instantiate(linePrefab, transform);
            currentColor = startTile.DotColor;
            SetLineColor(currentLine, currentColor);
            Debug.Log("Starting new path with color: " + currentColor);
            currentLine.positionCount = 1;
            currentLine.SetPosition(0, startTile.transform.position);
            
        }
    }

    public void ExtendPath(Tile nextTile)
    {
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Tile tile = GetTileUnderPosition(mouseWorldPos);

        if (tile != null && !currentPath.Contains(tile))
        {
            Tile lastTile = currentPath[currentPath.Count - 1];
            if (IsAdjacent(lastTile.GridPosition, tile.GridPosition))
            {
                if (tile.IsOccupied)
                {
                    if(tile.gameObject.layer == LayerMask.NameToLayer("Tile With Dot"))
                    {
                        Debug.Log("Tile is occupied by a dot.");
                        DestroyCurrentPath();
                    }
                    else
                    {
                        Debug.Log("Tile is occupied by another path.");
                        GridManager.Instance.LineDestroyed(tile.DotColor);
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
    }

    public void FinishPath()
    {
        if (currentPath.Count < 2)
        {
            DestroyCurrentPath();
            return;
        }

        Tile lastTile = currentPath[currentPath.Count - 1];
        if (lastTile.IsDot && lastTile.DotColor == currentColor && lastTile != startTile)
        {
            GridManager.Instance.SaveLinePath(currentPath, currentColor);
            GridManager.Instance.ColorsConnected(currentColor);
            Debug.Log("Connected: " + currentColor);
        }
        else
        {
            Debug.Log("Path not completed or color mismatch.");
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

    private Tile GetTileUnderPosition(Vector2 position)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero);
        if (hit.collider != null)
        {
            return hit.collider.GetComponent<Tile>();
        }
        return null;
    }

    private bool IsAdjacent(Vector2Int a, Vector2Int b)
    {
        return (Mathf.Abs(a.x - b.x) == 1 && a.y == b.y) || (Mathf.Abs(a.y - b.y) == 1 && a.x == b.x);
    }

    private void UpdateLineRenderer()
    {
        currentLine.positionCount = currentPath.Count;
        for (int i = 0; i < currentPath.Count; i++)
        {
            currentLine.SetPosition(i, currentPath[i].transform.position);
        }
    }

    public void RemoveLastPathStep(Tile tile)
    {
        if (currentPath.Count < 2) return;

        Tile lastTile = currentPath[currentPath.Count - 1];
        if (lastTile == tile)
        {
            lastTile.IsOccupied = false;
            lastTile.Color = "";
            currentPath.RemoveAt(currentPath.Count - 1);
            UpdateLineRenderer();
        }
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
