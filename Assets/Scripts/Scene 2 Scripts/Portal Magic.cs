using UnityEngine;

public class PortalMagic : MonoBehaviour, IInteractable
{

    [SerializeField] private int nextScene;

    public void Interact()
    {
        SceneLoader.Instance.PortalTeleport(nextScene);
    }



}
