using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }
    private bool hasFinishedCompassPuzzle = false;
    private bool hasSeenLetter = false;
    private bool hasTablet = false;
    private bool hasSeenTabletTutorial = false;
    private Vector2? playerPosition;
    private bool loadPosition = false;
    [SerializeField] private float cooldown = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (loadPosition)
        {
            Debug.LogWarning("Scene loaded: " + scene.name + " — restoring player position.");
            PlayerControls.Instance.SetPlayerPosition(playerPosition);
            loadPosition = false;
        }
    }
    private void LoadNextScene(int nextScene )
    {
        StartCoroutine(LoadNextSceneWaitCooldown(nextScene));
    }
    private IEnumerator LoadNextSceneWaitCooldown(int nextScene)
    {
        yield return new WaitForSeconds(cooldown);
        SceneManager.LoadSceneAsync(nextScene);

        
    }
    public void SetGameStage(int stage, int condition = 0)
    {
        //if condition is 1 the game will save the player position if its 2 it will load the scene with the last position, used for the last cutscene
        
        switch (condition)
        {
            case 0:
                //do nothing
                break;
            case 1:
                playerPosition = PlayerControls.Instance.GetPlayerPosition();
                break;
            case 2:
                loadPosition = true;
                break;

            default:
                
                break;
        }
        LoadNextScene(stage);
    }
    public void PortalTeleport(int nextScene)
    {
        StartCoroutine(LoadNextSceneWaitCooldown(nextScene));
    }

    public bool GetHasFinishedCompassPuzzle()
    {
        return hasFinishedCompassPuzzle;
    }
    public bool GetHasSeenLetter()
    {
        return hasSeenLetter;
    }
    public bool GetHasTablet()
    {
        return hasTablet;
    }
    public bool GetHasSeenTabletTutorial()
    {
        return hasSeenTabletTutorial;
    }
    public void SetHasFinishedCompassPuzzleTrue()
    {
        hasFinishedCompassPuzzle = true;
    }
    public void SetHasSeenLetterTrue()
    {
        hasSeenLetter = true;
    }
    public void SetHasTabletTrue()
    {
        hasTablet = true;
    }
    public void SetHasSeenTabletTutorialTrue()
    {
        hasSeenTabletTutorial = true;
    }

    public void SavePlayerPosition()
    {
        playerPosition = PlayerControls.Instance.GetPlayerPosition();
        Debug.Log("Saving player position " + this.transform.position);
        
    }



    public void DestroySingleton()
    {
        Instance = null;
        Destroy(gameObject);
    }


}
