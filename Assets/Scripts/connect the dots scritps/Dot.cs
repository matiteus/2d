using UnityEngine;

public class Dot : MonoBehaviour
{
    private string DotColor;
    public Tile TileUnderneath { get; private set; }
    [SerializeField]
    private LayerMask tileLayer;


    private void Awake()
    {
        DotColor = gameObject.tag;
    }
    //dot script
    private void Start()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down,1f,  tileLayer);
        if (hit.collider != null)
        { 
            TileUnderneath = hit.collider.GetComponent<Tile>();
            if (TileUnderneath != null)
            {
                TileUnderneath.SetIsDot(true);
                TileUnderneath.SetDotColor(DotColor);
                TileUnderneath.SetIsOccupied(true);
                TileUnderneath.gameObject.layer = LayerMask.NameToLayer("Tiles With Dots");
            }
            else
            {
                Debug.LogWarning("(DOT.CS) No Tile component found on the hit object. in " + gameObject.name + " hit object was: " + hit.collider.name);
            }
        }
        else
        {
            Debug.LogWarning("(DOT.CS) No Tile found underneath the Dot. in " + gameObject.name);
        }
    }
}
