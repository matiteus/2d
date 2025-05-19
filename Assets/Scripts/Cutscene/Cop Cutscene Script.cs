using UnityEngine;

public class CopCutsceneScript : Dialogue
{
    [SerializeField] private int nextScene = 2;

    private bool copDialogueStarted = false;
    protected override bool isDialoguePlaying
    {
        get => copDialogueStarted;
        set => copDialogueStarted = value;
    }

    protected override void HandleDialogueEnd()
    {
        
        if (copDialogueStarted)
        {
            SceneLoader.Instance.SetGameStage(nextScene);
        }

    }

    
}