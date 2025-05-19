using UnityEngine;

public class Letter : NPC
{

    [SerializeField] private DialogueText letterTutorial;

    private bool hasReadLetter = false;
    protected override bool hasInteracted
    {
        get => hasReadLetter;
        set => hasReadLetter = value;
    }

    protected override void HandleDialogueEnd()
    {
        if (!hasReadLetter)
        {
            hasReadLetter = true;
            Talk(letterTutorial);
        }
        else
        {
            PlayerControls.Instance.EnableReReadLetter();
            PlayerControls.Instance.EnableMovement();
        }
        
    }

}
