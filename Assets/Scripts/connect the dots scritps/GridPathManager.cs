using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GridPathManager : MonoBehaviour
{

    public static GridPathManager Instance { get; private set; }
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private LineRenderer linePrefab; // LineRenderer prefab for drawing path lines
    private Dictionary<Vector2Int, Tile> gridTiles = new Dictionary<Vector2Int, Tile>();

    private Tile startTile;
    private Tile endTile;
    private List<Tile> currentPath = new List<Tile>();
    private LineRenderer currentLine;

    private bool isDrawing = false;
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
            isDrawing = true;
            currentPath.Clear();
            currentPath.Add(startTile);

            currentLine = Instantiate(linePrefab, transform);
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
                    // If the tile already has a path, paths cross -> destroy both
                    DestroyPath(tile);
                    DestroyCurrentPath();
                }
                else
                {
                    currentPath.Add(tile);
                    tile.IsOccupied = true;
                    UpdateLineRenderer();
                }
            }
        }
    }

    public void FinishPath()
    {
        isDrawing = false;
        if (currentPath.Count < 2)
        {
            DestroyCurrentPath();
            return;
        }

        Tile lastTile = currentPath[currentPath.Count - 1];
        if (lastTile.IsDot && lastTile.DotColor == currentColor && lastTile != startTile)
        {
            // Success! Connected two dots
            GridManager.Instance.ColorsConnected(currentColor);
            Debug.Log("Connected: " + currentColor);
        }
        else
        {
            // Failed, path invalid
            DestroyCurrentPath();
        }
    }

    private void DestroyCurrentPath()
    {
        foreach (Tile tile in currentPath)
        {
            tile.IsOccupied = false;
        }
        if (currentLine != null)
        {
            Destroy(currentLine.gameObject);
        }
        currentPath.Clear();
    }

    private void DestroyPath(Tile tile)
    {
        // Optional: if you store tile's path line ref, destroy that too
        Debug.Log("Paths crossed! Both destroyed!");
        GridManager.Instance.LineDestroyed(tile.DotColor);
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
}
