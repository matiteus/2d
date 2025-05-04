using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class HandControl : MonoBehaviour
{
    [SerializeField] private LayerMask dotLayer;
    [SerializeField] private LayerMask tileLayer;
    public static HandControl Instance { get; private set; }

    private bool isDrawing = false;
    private List<Tile> currentPath = new List<Tile>();
    private Tile currentTile;

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
            isDrawing = true;
            currentTile = tile;
            GridPathManager.Instance.StartNewPath(currentTile);
        }
    }

    private void TryExtendDrawing()
    {
        Tile tile = GetTileUnderMouse(tileLayer);
        if(tile!= currentTile && tile)
        {
            currentTile = tile;
        }
        else
        {
            return;
        }
        GridPathManager.Instance.ExtendPath(currentTile);

    }

    private void EndDrawing()
    {
        isDrawing = false;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, LayerMask.GetMask("Tiles With Dots"));
        if (hit)
        {
            Tile lastTile = hit.collider.GetComponent<Tile>();
            GridPathManager.Instance.FinishPath(lastTile);
        }
        else
        {
            Debug.Log("No tile found.");
            GridPathManager.Instance.FinishPath(null);
            currentPath.Clear();
        }
    }
    private Tile GetTileUnderMouse(LayerMask mask)
    {
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero, mask);
        if (hit.collider != null)
        {
            Debug.Log(hit.collider.name);
            Debug.Log(hit.collider.gameObject.layer);
            if (hit.collider.TryGetComponent(out Dot dot))
            {       

                if (dot.gameObject.layer != LayerMask.NameToLayer("Connected Dots"))
                {
                    return dot.TileUnderneath;
                }
                else
                {
                    Debug.LogWarning("Hit a connected dot.");
                    return null;
                }
            }
            else if (hit.collider.TryGetComponent(out Tile tile))
            {
                return tile;
            }
        }
        return null;
    }

    

    public void LineDestroyed()
    {
        Debug.Log("Line destroyed.");
        isDrawing = false;
    }
}
