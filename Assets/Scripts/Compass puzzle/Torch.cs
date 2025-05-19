using UnityEngine;

public class Torch : MonoBehaviour, IInteractable
{
    [SerializeField]
    private string torchLocation = "" ;
    private bool isActivated = false;
    private SpriteRenderer mySpriteRenderer;

    //torch script
    private void Awake()
    {
        torchLocation = this.name;
        mySpriteRenderer = GetComponent<SpriteRenderer>();

    }


    public void Interact()
    {
        if(!isActivated)
        {
            ActivateTorch();
            CompassManager.Instance.AddToTheListOfTorches(this, torchLocation);
        }
        

    }

    public void ResetTorch()
    {
        isActivated = false;
        mySpriteRenderer.color = Color.white;
    }
    public void ActivateTorch()
    {
        if(!mySpriteRenderer)
        {
            mySpriteRenderer = GetComponent<SpriteRenderer>();
        }
        mySpriteRenderer.color = Color.red;
        isActivated = true;
    }
}
