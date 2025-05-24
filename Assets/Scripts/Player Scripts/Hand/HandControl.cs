using UnityEngine;
using UnityEngine.InputSystem;


public class HandControl : MonoBehaviour
{
    [SerializeField] private LayerMask dotLayer;
    [SerializeField] private LayerMask tileLayer;
    private Controls controls;
    [SerializeField] private GameObject canvaOverlay;
    public static HandControl Instance { get; private set; }

    private bool isDrawing = false;
    private Tile currentTile;
    private bool skippedTutorial = false;

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


        controls = new Controls();

        // Register input callbacks
        controls.Mouse.LeftClick.performed += OnLeftClick;
        //controls.Mouse.RightClick.performed += OnRightClick;
    }

    private void Start()
    {
        
    }




//hand script

    private void OnEnable()
    {
        controls.Mouse.Enable();
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        controls.Mouse.Disable();
        Cursor.visible = true;
    }

    private void OnLeftClick(InputAction.CallbackContext context)
    {
        if (!skippedTutorial)
        {
            canvaOverlay.SetActive(false);
            skippedTutorial = true;
        }
        else
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
        

    }

    /*private void OnRightClick(InputAction.CallbackContext context)
    {

    }*/

    private void FixedUpdate()
    {
        if (Mouse.current == null) return;

        Vector3 mousePos = Mouse.current.position.ReadValue();
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        mousePos.z = 1;

        transform.position = mousePos;


        if (isDrawing)
        {
            TryExtendDrawing();
        }
    }


    private void TryStartDrawing()
    {
        Tile tile = GetTileUnderMouse(dotLayer);

        if (tile != null && tile.GetIsDot()) 
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
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down,1f, LayerMask.GetMask("Tiles With Dots"));
        if (hit)
        {
            Tile lastTile = hit.collider.GetComponent<Tile>();
            GridPathManager.Instance.FinishPath(lastTile);
        }
        else
        {
            Debug.Log("No tile found.");
            GridPathManager.Instance.FinishPath(null);

        }
    }
    private Tile GetTileUnderMouse(LayerMask mask)
    {
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero, 1f, mask);    
        if (hit.collider != null)
        {

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
