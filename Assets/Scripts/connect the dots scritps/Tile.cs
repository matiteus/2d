using UnityEngine;

public class Tile : MonoBehaviour
{
    private Vector2Int gridPosition;
    private bool isOccupied = false;
    private string color = "";
    private bool isDot = false;
    private string dotColor = "";

    public void SetGridPosition(Vector2Int pos)
    {
        gridPosition = pos;
    }
    public void SetIsOccupied(bool occupied)
    {
        isOccupied = occupied;
    }
    public void SetColor(string col)
    {
        color = col;
    }
    public void SetIsDot(bool dot)
    {
        isDot = dot;
    }
    public void SetDotColor(string color)
    {
        dotColor = color;
    }
    public Vector2Int GetGridPosition()
    {
        return gridPosition;
    }
    public bool GetIsOccupied()
    {
        return isOccupied;
    }
    public string GetColor()
    {
        return color;
    }
    public bool GetIsDot()
    {
        return isDot;
    }
    public string GetDotColor()
    {
        return dotColor;
    }


}