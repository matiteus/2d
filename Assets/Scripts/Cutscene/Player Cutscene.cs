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

    protected override void HandleDialogueEnd()
    {
        if (playerMonologueStarted)
        {   //second scene for first cutscene
            if (cutscene2)
            {
                cutscene2.gameObject.SetActive(true);
            }
            else
            {
                if (nextScene == 5)
                {
                    //for the second cutscene
                    SceneLoader.Instance.SetGameStage(nextScene);
                }
                else
                {
                    //for the third and last cutscene
                    SceneLoader.Instance.SetGameStage(nextScene, 2);
                }

            }

        }
    }
}
