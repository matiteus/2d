public class Cop : Interactable
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
            hasInteractedWithCop = false;
        }
    }

}
