using UnityEngine;

public class ReReadLetter : MonoBehaviour
{
    public static ReReadLetter Instance { get; private set; } = null;

    [SerializeField] private DialogueText letterDialogue;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void ReadLetter()
    {
        if (letterDialogue != null)
        {
            DialogueControllerScript.Instance.StartConversation(letterDialogue);
        }
        else
        {
            Debug.LogWarning("Letter dialogue is not assigned.");
        }
    }
}
