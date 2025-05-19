using UnityEngine;

public class LeadInteractable : NPC
{
    [SerializeField] private int nextScene = 3;

    private bool hasInteractedWithLead = false;
    protected override bool hasInteracted
    {
        get => hasInteractedWithLead;
        set => hasInteractedWithLead = value;
    }

    protected override void HandleDialogueEnd()
    {
        if (hasInteractedWithLead)
        {
            PlayerControls.Instance.EnableMovement();
            SceneLoader.Instance.SetGameStage(nextScene);
        }
        
    }




}
