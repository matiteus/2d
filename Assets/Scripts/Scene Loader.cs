using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }
    public bool hasFinishedCompassPuzzle = false;
    [SerializeField] private float cooldown = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void LoadNextScene(int nextScene)
    {
        StartCoroutine(LoadNextSceneWaitCooldown(nextScene));
    }
    private IEnumerator LoadNextSceneWaitCooldown(int nextScene)
    {
        yield return new WaitForSeconds(cooldown);
        SceneManager.LoadSceneAsync(nextScene);
    }
    public void SetGameStage(int stage)
    {
        LoadNextScene(stage);
    }
    public void PortalTeleport(int nextScene)
    {
        Debug.Log("Teleporting to scene: " + nextScene);
        StartCoroutine(LoadNextSceneWaitCooldown(nextScene));
    }

}
