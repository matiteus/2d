using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class GridManager : MonoBehaviour
{
    private static int lastStage = 2;

    public static GridManager Instance { get; private set; }
    [SerializeField]
    private GameObject grid1;
    [SerializeField]
    private GameObject grid2;
    [SerializeField]
    private GameObject grid3;
    private GameObject instantiatedGrid;
    private GameObject blueDot1;
    private GameObject blueDot2;
    private GameObject greedDot1;
    private GameObject greedDot2;
    private GameObject redDot1;
    private GameObject redDot2;
    private Dictionary<string, List<Tile>> paths = new Dictionary<string, List<Tile>>();
    private int connectionsMade = 0;
    private int gameState = 0; // 0 = grid1, 1 = grid2, 2 = grid3


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
        Debug.Log("GridManager Start called");
        instantiatedGrid = Instantiate(grid1, transform);
        FindChldrenWithTag(instantiatedGrid.transform);
    }

    private void FindChldrenWithTag(Transform grid)
    {
        bool foundBlue = false;
        bool foundGreen = false;
        bool foundRed = false;

        foreach (Transform child in grid)
        {
            if (child.CompareTag("Blue Dot"))
            {
                if (!foundBlue)
                {
                    blueDot1 = child.gameObject;
                    foundBlue = true;
                }
                else
                {
                    blueDot2 = child.gameObject;
                }
            }
            else if (child.CompareTag("Green Dot"))
            {
                if (!foundGreen)
                {
                    greedDot1 = child.gameObject;
                    foundGreen = true;
                }
                else
                {
                    greedDot2 = child.gameObject;
                }
            }
            else if (child.CompareTag("Red Dot"))
            {
                if (!foundRed)
                {
                    redDot1 = child.gameObject;
                    foundRed = true;
                }
                else
                {
                    redDot2 = child.gameObject;
                }
            }
        }
    }

    public void ColorsConnected(string color)
    {
        Debug.Log("ColorsConnected called with color: " + color);
        if (color == "Blue")
        {
            Debug.Log("bluedot1 layer is " + blueDot1.layer);
            blueDot1.layer = LayerMask.NameToLayer("Connected Dots");
            Debug.Log("bluedot1 layer is " + blueDot1.layer);
            blueDot2.layer = LayerMask.NameToLayer("Connected Dots");
            connectionsMade++;
        }
        else if (color == "Green")
        {
            Debug.Log("greendot1 layer is " + greedDot1.layer);
            greedDot1.layer = LayerMask.NameToLayer("Connected Dots");
            Debug.Log("greendot1 layer is " + greedDot1.layer);
            greedDot2.layer = LayerMask.NameToLayer("Connected Dots");
            connectionsMade++;
        }
        else if (color == "Red")
        {
            Debug.Log("reddot1 layer is " + redDot1.layer);
            redDot1.layer = LayerMask.NameToLayer("Connected Dots");
            Debug.Log("reddot1 layer is " + redDot1.layer);
            redDot2.layer = LayerMask.NameToLayer("Connected Dots");
            connectionsMade++;
        }

    }

    public void LineDestroyed(string color)
    {
        if (color == "Blue")
        {
            blueDot1.layer = LayerMask.NameToLayer("Dots");
            blueDot2.layer = LayerMask.NameToLayer("Dots");
            connectionsMade--;
        }
        else if (color == "Green")
        {
            greedDot1.layer = LayerMask.NameToLayer("Dots");
            greedDot2.layer = LayerMask.NameToLayer("Dots");
            connectionsMade--;
        }
        else if (color == "Red")
        {
            redDot1.layer = LayerMask.NameToLayer("Dots");
            redDot2.layer = LayerMask.NameToLayer("Dots");
            connectionsMade--;
        }
        DeleteLinePath(color);

    }

    public void SaveLinePath(List<Tile> linePath, string color)
    {
        paths[color] = linePath;
    }
    private void DeleteLinePath(string color)
    {
        foreach (var path in paths[color])
        {
            path.IsOccupied = false;
            path.Color = "";    
        }
    }
    private void CheckLoadNextStage()
    {
        if(gameState == lastStage)
        {
            //load cutscene
        }
        else
        {
            Destroy(instantiatedGrid);

            gameState++;
            connectionsMade = 0;
            FindChldrenWithTag(instantiatedGrid.transform);
        }
    }
}

   