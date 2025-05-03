using UnityEngine;


[CreateAssetMenu(menuName = "Dialogue/New Dialogue Container")]
public class DialogueText :ScriptableObject
{
    public string characterName;
    [TextArea(5,10)]
    public string[] dialogue;
}
