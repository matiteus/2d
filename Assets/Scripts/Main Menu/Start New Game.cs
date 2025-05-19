using UnityEngine;


public class StartNewGame : MonoBehaviour
{
    [SerializeField] private int gameStage = 1;
    public void PlayGame()
    {
        SceneLoader.Instance.SetGameStage(gameStage);
    }
   
}
