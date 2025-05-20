using UnityEngine;

public class PortalEndGame : MonoBehaviour,IInteractable
{
    [SerializeField] private int nextScene;
    public void Interact()
    {
        SceneLoader.Instance.PortalTeleport(nextScene);
    }
}
