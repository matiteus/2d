using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalEndGame : MonoBehaviour,IInteractable
{
    public void Interact()
    {
        StartCoroutine(LoadNextCutscene());
    }

    private IEnumerator LoadNextCutscene()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadSceneAsync(9);
    }

}
