using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class NPC : MonoBehaviour, IInteractable
{
    [SerializeField] private SpriteRenderer interactSprite;
    [SerializeField] private float interactDistance = 5f;
    [SerializeField] private Transform player;



    // Update is called once per frame
    void  FixedUpdate()
    {
        if(Keyboard.current.eKey.wasPressedThisFrame && !IsWithinInteractDistance())
        {
            Interact();
        }
        if (interactSprite.gameObject.activeSelf && !IsWithinInteractDistance())
        {
            interactSprite.gameObject.SetActive(false);
        }
        else if(!interactSprite.gameObject.activeSelf && IsWithinInteractDistance())
        {
            interactSprite.gameObject.SetActive(true);
        }
    }
    public abstract void Interact();

    private bool IsWithinInteractDistance()
    {
        return Vector2.Distance(player.position, transform.position) < interactDistance;
    }
}
