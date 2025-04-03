using UnityEngine;

public class PlayerInteractableHandler : MonoBehaviour
{
    private SpriteRenderer mySpriteRenderer;
    private bool canInteract = false;
    private Collider2D myCollider2D;


    private void Start()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        myCollider2D = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("interactable"))
        {
            mySpriteRenderer.enabled = true;
            canInteract = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        mySpriteRenderer.enabled = false;
        canInteract = false;
    }
    public void StartInteraction()
    {
        if (canInteract)
        {
            Debug.Log("u interacted wooow");
        }
        else
        {
            Debug.Log("u cant interact");
        }
    }
}
