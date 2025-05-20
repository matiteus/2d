using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayAgainButton : MonoBehaviour
{
    public void OnPlayAgainButtonClicked()
    {
        SceneLoader.Instance.DestroySingleton();

        SceneManager.LoadScene(0);
    }
}

