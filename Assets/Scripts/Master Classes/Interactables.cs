using UnityEngine;

public abstract class Dialogue : MonoBehaviour
{
    protected abstract bool isDialoguePlaying { get; set; }

    //has a signal in the timeline that will trigger on cutscenes
    // subclass NPC serializes the DialogueText
    public void Talk(DialogueText dialogueText)
    {
        isDialoguePlaying = true;
        DialogueControllerScript.Instance.StartConversation(dialogueText);
    }

    private void OnEnable()
    {
        DialogueControllerScript.Instance.OnConversationEnded += HandleDialogueEnd;
    }

    private void OnDisable()
    {
        DialogueControllerScript.Instance.OnConversationEnded -= HandleDialogueEnd;

    }
    // enables the next cutscene after the dialogue ends
    protected abstract void HandleDialogueEnd();
}
