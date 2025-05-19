public class Cop : NPC
{

    private bool hasInteractedWithCop = false;
    protected override bool hasInteracted
    {
        get => hasInteractedWithCop;
        set => hasInteractedWithCop = value;
    }


    protected override void HandleDialogueEnd()
    {
        if (hasInteractedWithCop)
        {
            PlayerControls.Instance.EnableMovement();
            hasInteractedWithCop = false;
        }
    }

}
