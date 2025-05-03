using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class HandControl : MonoBehaviour
{
    [SerializeField] private LayerMask dotLayer;
    [SerializeField] private LayerMask tileLayer;

    private bool isDrawing = false;
    private Tile startTile;
    private List<Tile> currentPath = new List<Tile>();

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
            Debug.Log("Left mouse button pressed, calling TryStartDrawing");
            TryStartDrawing();
        }
        else if (Mouse.current.leftButton.isPressed && isDrawing)
        {
            Debug.Log("Left mouse button is pressed, calling TryExtendDrawing");
            TryExtendDrawing();
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame && isDrawing)
        {
            Debug.Log("Left mouse button released, calling EndDrawing");
            EndDrawing();
        }
    }

    private void TryStartDrawing()
    {
        Tile tile = GetTileUnderMouse(dotLayer);

        if (tile != null && tile.IsDot)
        {
            startTile = tile;
            isDrawing = true;
            currentPath.Clear();
            GridPathManager.Instance.StartNewPath(startTile);
        }
        else
        {
            Debug.Log("Tile is null or not a dot, cannot start drawing.");
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
    }

    private void EndDrawing()
    {
        isDrawing = false;
        GridPathManager.Instance.FinishPath();
        currentPath.Clear();
    }

    private Tile GetTileUnderMouse(LayerMask mask)
    {
        Debug.Log("GetTileUnderMouse called");
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero, mask);
        if (hit.collider != null)
        {
            if (hit.collider.TryGetComponent(out Dot dot))
            {
                Debug.Log("Hit a dot");
                return dot.TileUnderneath; // Use the tile under the dot
            }
            else if (hit.collider.TryGetComponent(out Tile tile))
            {
                Debug.Log("Hit a tile");
                return tile; // Directly clicked a tile
            }
        }
        Debug.Log("No collider hit");
        return null;
    }

    private bool IsAdjacent(Tile a, Tile b)
    {
        Vector2Int diff = a.GridPosition - b.GridPosition;
        return (Mathf.Abs(diff.x) == 1 && diff.y == 0) || (Mathf.Abs(diff.y) == 1 && diff.x == 0);
    }
}
