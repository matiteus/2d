using UnityEngine;

public class PortalMagic : MonoBehaviour
{

    [SerializeField] private int nextScene;

    public void Interact()
    {
        SceneLoader.Instance.PortalTeleport(nextScene);
    }



}
