using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class HandControl : MonoBehaviour
{
    [SerializeField] private LayerMask dotLayer;
    [SerializeField] private LayerMask tileLayer;
    public static HandControl Instance { get; private set; }

    private bool isDrawing = false;
    private Tile startTile;
    private List<Tile> currentPath = new List<Tile>();

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
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Mouse.current == null) return;

        Vector3 mousePos = Mouse.current.position.ReadValue();
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        mousePos.z = 0;

        transform.position = mousePos;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (!isDrawing)
            {
                TryStartDrawing();
            }
            else
            {
                EndDrawing();
            }
        }

        if (isDrawing)
        {
            TryExtendDrawing();
        }
    }


    private void TryStartDrawing()
    {
        Tile tile = GetTileUnderMouse(dotLayer);

        if (tile != null && tile.IsDot)
        {
            startTile = tile;
            isDrawing = true;
            GridPathManager.Instance.StartNewPath(startTile);
        }
    }

    private void TryExtendDrawing()
    {
        Tile tile = GetTileUnderMouse(tileLayer); // Only check Tiles

        if (tile != null && !currentPath.Contains(tile))
        {
            Tile lastTile = currentPath.Count > 0 ? currentPath[currentPath.Count - 1] : startTile;

            if (IsAdjacent(lastTile, tile))
            {
                currentPath.Add(tile);
                GridPathManager.Instance.ExtendPath(tile);
            }
        }
        else if (currentPath.Count >= 2 && tile == currentPath[currentPath.Count - 2])
        {
            // Remove the last tile (we are stepping back)
            Tile removed = currentPath[currentPath.Count - 1];
            currentPath.RemoveAt(currentPath.Count - 1);

            GridPathManager.Instance.RemoveLastPathStep(removed);
        }
    }

    private void EndDrawing()
    {
        isDrawing = false;
        GridPathManager.Instance.FinishPath();
        currentPath.Clear();
    }

    private Tile GetTileUnderMouse(LayerMask mask)
    {
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero, mask);
        if (hit.collider != null)
        {
            if (hit.collider.TryGetComponent(out Dot dot))
            {
                return dot.TileUnderneath; 
            }
            else if (hit.collider.TryGetComponent(out Tile tile))
            {
                return tile;
            }
        }
        return null;
    }

    private bool IsAdjacent(Tile a, Tile b)
    {
        Vector2Int diff = a.GridPosition - b.GridPosition;
        return (Mathf.Abs(diff.x) == 1 && diff.y == 0) || (Mathf.Abs(diff.y) == 1 && diff.x == 0);
    }

    public void LineDestroyed()
    {
        isDrawing = false;
    }
}
