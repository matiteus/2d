using UnityEngine;

public class Dot : MonoBehaviour
{
    private string DotColor;
    public Tile TileUnderneath { get; private set; }


    private void Awake()
    {
        // Set the color of the dot based on its name
        if (gameObject.name.Contains("Blue"))
        {
            DotColor = "Blue";
        }
        else if (gameObject.name.Contains("Green"))
        {
            DotColor = "Green";
        }
        else if (gameObject.name.Contains("Red"))
        {
            DotColor = "Red";
        }
        else
        {
            DotColor = "Unknown";
        }
    }
    private void Start()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down,  LayerMask.GetMask("Tile")); // Only hit Tile layer
        if (hit.collider != null)
        { 
            TileUnderneath = hit.collider.GetComponent<Tile>();
            if (TileUnderneath != null)
            {
                TileUnderneath.IsDot = true;
                TileUnderneath.DotColor = DotColor;
                TileUnderneath.gameObject.layer = LayerMask.NameToLayer("Tile With Dot");
                TileUnderneath.IsOccupied = true;
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
