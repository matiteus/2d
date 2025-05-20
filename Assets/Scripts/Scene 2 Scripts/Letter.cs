using UnityEngine;

public class Letter : Interactable
{

    private bool hasReadLetter = false;


    protected override bool hasInteracted
    {
        get => hasReadLetter;
        set => hasReadLetter = value;
    }
    private void Awake()
    {
        if (SceneLoader.Instance.GetHasSeenLetter())
        {
            this.transform.parent.gameObject.SetActive(false);
        }
    }


    protected override void HandleDialogueEnd()
    {
        if (hasReadLetter)
        {
            GetComponent<LetterTutorial>().TriggerLetterTutorial();
        }
    }


}
