using UnityEngine;

public class LeadInteractable : NPC, ITalkable
{
    [SerializeField] private DialogueText dialogueText;
    [SerializeField] private DialogueControllerScript dialogueController;
    private bool hasStartedConversation = false;


    private void Start()
    {
        dialogueController.OnConversationEnded += () =>
        {
            hasStartedConversation = false;
        };
    }

    public override void Interact()
    {
        if (!hasStartedConversation)
        {
            hasStartedConversation = true;
            dialogueController.gameObject.SetActive(true);
            Talk(dialogueText);
        }
    }

    public void Talk(DialogueText dialogueText)
    {
        dialogueController.StartConversation(dialogueText);
    }
}
