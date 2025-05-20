using UnityEngine;


public class Portal : MonoBehaviour, IInteractable
{

    [SerializeField] private int nextScene;

    public void Interact()
    { 
    SceneLoader.Instance.PortalTeleport(nextScene);
       
    }
}
