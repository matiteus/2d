using UnityEngine;


public class Portal : MonoBehaviour, IInteractable
{

    [SerializeField] private int nextScene;

    public void Interact()
    {
        if(SceneLoader.Instance.hasFinishedCompassPuzzle)
        {
            SceneLoader.Instance.SetGameStage(nextScene+2);
        }
        else
        {
            SceneLoader.Instance.PortalTeleport(nextScene);
        }
       
    }




}
