using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Interactable : Dialogue, IInteractable
{
    [SerializeField] private SpriteRenderer interactSprite;
    [SerializeField] private DialogueText dialogueText;

    private float timeToWaitForCutscene = 1f;
    protected abstract bool hasInteracted { get; set; }
    protected override bool isDialoguePlaying
    {
        get => hasInteracted;
        set => hasInteracted = value;
    }

    public void Interact()
    {
        hasInteracted = true;
        Talk(dialogueText);
    }

    protected IEnumerator LoadNextScene(int cutscene)
    {
        yield return new WaitForSeconds(timeToWaitForCutscene);
        SceneManager.LoadSceneAsync(cutscene);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactSprite.enabled = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactSprite.enabled = false;
        }
    }

}
