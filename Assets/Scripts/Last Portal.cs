using UnityEngine;

public class LastPortal : MonoBehaviour
{
    [SerializeField] private GameObject portal;

    private void Awake()
    {
        if (SceneLoader.Instance.GetHasFinishedCompassPuzzle())
        {
            portal.SetActive(true);
        }

        else
        {
            portal.SetActive(false);
        }
    }
}
