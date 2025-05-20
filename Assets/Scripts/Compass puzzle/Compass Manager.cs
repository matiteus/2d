using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CompassManager : MonoBehaviour
{
    public static CompassManager Instance { get; private set; }
    [SerializeField] private List<string> expectedList = new();
    [SerializeField] private int nextScene = 7;
    private List<Torch> listOfTorchesScript = new();
    private List<string> listOfTorches = new();
    bool hasFinished = false;



    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (SceneLoader.Instance.GetHasFinishedCompassPuzzle())
        {
            hasFinished = true;
            ActivateAllTorches();
        }
    }


    //compass manager
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!hasFinished)
        {
            ResetTorches();
        }    

    }


    public void AddToTheListOfTorches(Torch torch ,string position)
    {
        listOfTorches.Add(position);
        listOfTorchesScript.Add(torch);
        CheckForCompletion();
        
    }

    private void CheckForCompletion()
    {
        if(listOfTorches.Count == expectedList.Count)
        {
            if (listOfTorches.SequenceEqual(expectedList))
            {
                SceneLoader.Instance.SetHasFinishedCompassPuzzleTrue();
                Debug.Log("torches activated sucessfully");
                ActivateAllTorches();
                SceneLoader.Instance.SetGameStage(nextScene, 1);

            }
            else
            {
                ResetTorches();
            }
        }
        else if(listOfTorches.Count > expectedList.Count)
        {
            ResetTorches();
        }
    }


    private void ResetTorches()
    {
        Debug.Log("torches activated in the wrong order");
        foreach (Torch torch in listOfTorchesScript)
        {
            torch.ResetTorch();
        }
        listOfTorches.Clear();
        listOfTorchesScript.Clear();
    }

    public void ActivateAllTorches()
    {
        foreach (Torch torch in GetComponentsInChildren<Torch>())
        {
            torch.ActivateTorch();
        }
    }
}

