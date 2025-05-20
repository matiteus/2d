using UnityEngine;
using UnityEngine.InputSystem;

public class TabletTutorial : MonoBehaviour
{
    private Controls mycontrols;
    private void Awake()
    {
        if (SceneLoader.Instance.GetHasSeenTabletTutorial())
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            PlayerControls.Instance.DisableMovement();
            mycontrols = new Controls();
            mycontrols.Enable();
            mycontrols.Movement.Interact.performed += SkipTutorial;
        }
    }


    private void OnDisable()
    {
        if (mycontrols != null)
        {
            mycontrols.Movement.Interact.performed -= SkipTutorial;
        }
    }


    private void SkipTutorial(InputAction.CallbackContext context)
    {
        SceneLoader.Instance.SetHasSeenTabletTutorialTrue();
        PlayerControls.Instance.EnableMovement();
        this.gameObject.SetActive(false);
        mycontrols.Disable();
    }

}
