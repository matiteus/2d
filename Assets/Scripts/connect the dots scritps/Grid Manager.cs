using UnityEngine;
using UnityEngine.InputSystem;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }
    [SerializeField]
    private GameObject grid1;
    [SerializeField]
    private GameObject grid2;
    [SerializeField]
    private GameObject grid3;
    private GameObject blueDot1;
    private GameObject blueDot2;
    private GameObject greedDot1;
    private GameObject greedDot2;
    private GameObject redDot1;
    private GameObject redDot2;
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
        FindChldrenWithTag(grid1.transform);
        //Instantiate grid
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
        if (color == "Blue Dot")
        {
            blueDot1.GetComponent<BoxCollider2D>().enabled = false;
            blueDot2.GetComponent<BoxCollider2D>().enabled = false;
            connectionsMade++;
            CheckLoadNextStage();
        }
        else if (color == "Green Dot")
        {
            greedDot1.GetComponent<BoxCollider2D>().enabled = false;
            greedDot2.GetComponent<BoxCollider2D>().enabled = false;
            connectionsMade++;
            CheckLoadNextStage();
        }
        else if (color == "Red Dot")
        {
            redDot1.GetComponent<BoxCollider2D>().enabled = false;
            redDot2.GetComponent<BoxCollider2D>().enabled = false;
            connectionsMade++;
            CheckLoadNextStage();
        }

    }

    public void LineDestroyed(string color)
    {
        if (color == "Blue Dot")
        {
            blueDot1.GetComponent<BoxCollider2D>().enabled = true;
            blueDot2.GetComponent<BoxCollider2D>().enabled = true;
            connectionsMade--;
        }
        else if (color == "Green Dot")
        {
            greedDot1.GetComponent<BoxCollider2D>().enabled = true;
            greedDot2.GetComponent<BoxCollider2D>().enabled = true;
            connectionsMade--;
        }
        else if (color == "Red Dot")
        {
            redDot1.GetComponent<BoxCollider2D>().enabled = true;
            redDot2.GetComponent<BoxCollider2D>().enabled = true;
            connectionsMade--;
        }
    }

    private void CheckLoadNextStage()
    {
        if (gameState == 0)
        {
            if (connectionsMade == 3)
            {
                grid1.SetActive(false);
                grid2.SetActive(true);
                gameState = 1;
                FindChldrenWithTag(grid2.transform);
            }
        }
        else if (gameState == 1)
        {
            if (connectionsMade == 3)
            {
                grid2.SetActive(false);
                grid3.SetActive(true);
                gameState = 2;
                FindChldrenWithTag(grid3.transform);
            }
        }
        else if (gameState == 2)
        {
            if (connectionsMade == 3)
            {
                // Load next stage or do something else
            }

        }
    }
}

   