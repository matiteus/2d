using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System;

public class DialogueControllerScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI dialogueTextUI;

    public event Action OnConversationEnded;

    private Controls controls;
    private Queue<string> sentences;
    private bool conversationEnded = true;

    private void Awake()
    {
        sentences = new Queue<string>();
    }

    private void OnEnable()
    {
        controls = new Controls();
        controls.Enable();
        controls.Movement.Interact.performed += OnContinueInput;
    }

    private void OnDisable()
    {
        controls.Movement.Interact.performed -= OnContinueInput;
        controls.Disable();
    }

    public void StartConversation(DialogueText dialogueText)
    {
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        nameText.text = dialogueText.characterName;
        dialogueTextUI.text = "";

        conversationEnded = false;
        sentences.Clear();

        foreach (string line in dialogueText.dialogue)
        {
            sentences.Enqueue(line);
        }

        DisplayNextSentence();
    }

    private void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndConversation();
            return;
        }

        string nextSentence = sentences.Dequeue();
        dialogueTextUI.text = nextSentence;
    }


    private void EndConversation()
    {
        conversationEnded = true;
        dialogueTextUI.text = "";
        nameText.text = "";
        gameObject.SetActive(false);

        OnConversationEnded?.Invoke();
    }


    private void OnContinueInput(InputAction.CallbackContext context)
    {
        if (!gameObject.activeSelf || conversationEnded)
            return;

        DisplayNextSentence();
    }
}
