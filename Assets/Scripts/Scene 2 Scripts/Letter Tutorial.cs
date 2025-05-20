using UnityEngine;

public class LetterTutorial : Dialogue
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private DialogueText letterTutorial;

    private bool seenLetterTutorial = false;

    protected override bool isDialoguePlaying
    {
        get => seenLetterTutorial;
        set => seenLetterTutorial = value;
    }

    public void TriggerLetterTutorial()
    {
        Talk(letterTutorial);
    }

    protected override void HandleDialogueEnd()
    {
        if (seenLetterTutorial)
        {

            PlayerControls.Instance.EnableReReadLetter();
            SceneLoader.Instance.SetHasSeenLetterTrue();
            CrowManager.Instance.ActivateCompassCrow();
            this.transform.parent.gameObject.SetActive(false);

        }
    }
}

