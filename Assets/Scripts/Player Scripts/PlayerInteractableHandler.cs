using UnityEngine;

public class PlayerInteractableHandler : MonoBehaviour
{


    public void StartInteraction()
    {
        float interactRadius = 2f;
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactRadius);

        foreach (var hit in hits)
        {
            IInteractable interactable = hit.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact();
                break;
            }
        }
    }
}
