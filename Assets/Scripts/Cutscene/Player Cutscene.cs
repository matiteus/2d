using UnityEngine;
using UnityEngine.Playables;



public class PlayerCutscene : Dialogue
{
    [SerializeField] private PlayableDirector cutscene2;
    [SerializeField] private int nextScene = 5;

    private bool playerMonologueStarted = false;
    protected override bool isDialoguePlaying
    {
        get => playerMonologueStarted;
        set => playerMonologueStarted = value;
    }

    protected override void  HandleDialogueEnd()
    {
        if (playerMonologueStarted)
        {   //second scene for first cutscene
            if (cutscene2)
            {
                cutscene2.gameObject.SetActive(true);
            }
            else
            {
                //Start the second Cutscene
                SceneLoader.Instance.SetGameStage(nextScene);
            }
            
        }
        
    }
}
