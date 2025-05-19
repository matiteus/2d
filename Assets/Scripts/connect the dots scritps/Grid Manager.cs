using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GridManager : MonoBehaviour
{

    public static GridManager Instance { get; private set; }
    [SerializeField]
    private int nextScene = 4;
    [SerializeField]
    private int lastStage = 2;
    [SerializeField]
    private List<string> colors = new List<string> { "Blue", "Green", "Red" };
    private Dictionary<string, List<GameObject>> dotsColor = new Dictionary<string, List<GameObject>>();
    [SerializeField]
    private List<GameObject> grids = new List<GameObject>();
    private GameObject instantiatedGrid;
    private Dictionary<string, List<Tile>> paths = new Dictionary<string, List<Tile>>();
    private Dictionary<string, LineRenderer> lines = new Dictionary<string, LineRenderer>();
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
        instantiatedGrid = Instantiate(grids[gameState], transform);
        FindChildrenWithTag(instantiatedGrid.transform);
    }

    private void FindChildrenWithTag(Transform grid)
    {
        dotsColor.Clear();

        foreach (string color in colors)
        {
            dotsColor[color] = new List<GameObject>(2); 
        }

        foreach (Transform child in grid)
        {
            foreach (string color in colors)
            {
                if (child.CompareTag(color))
                {
                    var colorList = dotsColor[color];
                    if (colorList.Count < 2)
                    {
                        colorList.Add(child.gameObject);
                    }
                    break;
                }
            }
        }
    }

    //grid manager
    public void ColorsConnected(string color)
    {

        foreach (GameObject dot in dotsColor[color])
        {
            dot.layer = LayerMask.NameToLayer("Connected Dots");
        }

        connectionsMade++;

        if (connectionsMade == colors.Count)
        {
            CheckLoadNextStage();
        }
    }


    public void LineDestroyed(string color)
    {
        foreach (GameObject dot in dotsColor[color])
        {
            dot.layer = LayerMask.NameToLayer("Dots");
        }

        connectionsMade--;
        DeleteLinePath(color);

    }

    public void SaveLinePath(List<Tile> linePath, string color, LineRenderer line)
    {
        paths[color] = new List<Tile>(linePath);
        lines[color] = line;
    }
    private void DeleteLinePath(string color)
    {
        Debug.Log("DeleteLinePath called with color: " + color);
        foreach (var path in paths[color])
        {
            Debug.Log("checking tile " + path.gameObject.name);
            if (!path.GetIsDot())
            {
                path.SetIsOccupied(false);
                path.SetColor("");
            }
              
        }
        Destroy(lines[color].gameObject);
    }
    private void CheckLoadNextStage()
    {
        gameState++;
        Destroy(instantiatedGrid);
        StartCoroutine(LoadNextGrid());
        if (gameState == lastStage)
        {
            SceneLoader.Instance.SetGameStage(nextScene);
        }
    }
    private IEnumerator LoadNextGrid()
    {
        yield return new WaitForSeconds(0.5f);
        connectionsMade = 0;
        instantiatedGrid = Instantiate(grids[gameState], transform);
        FindChildrenWithTag(instantiatedGrid.transform);
    }
    
}

   