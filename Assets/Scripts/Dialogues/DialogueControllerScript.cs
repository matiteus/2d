using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System;
using System.Collections;


public class DialogueControllerScript : MonoBehaviour
{
    public static DialogueControllerScript Instance { get; private set; } = null;
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI dialogueTextUI;

    public event Action OnConversationEnded;

    private Controls controls;
    private Queue<string> sentences;
    private bool conversationEnded = true;
    //for some reason sometimes the interaction input triggers the next sentence on the dialogue, so this will wait for the next frame to allow input when dialogue starts
    private bool inputAllowed = false;



    private void Awake()
    {
        Debug.Log("DialogueControllerScript Awake");
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.Log(this.gameObject.name + " already exists, destroying this instance");
            Debug.Log("DialogueControllerScript already exists, destroying this instance");
            Destroy(gameObject);
        }
        if (sentences == null)
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
        if(PlayerControls.Instance)
        {
            PlayerControls.Instance.DisableMovement();
        }
        inputAllowed = false;
        if (!dialogueBox.activeSelf)
            dialogueBox.SetActive(true);

        nameText.text = dialogueText.characterName;
        dialogueTextUI.text = "";

        conversationEnded = false;
        sentences.Clear();

        foreach (string line in dialogueText.dialogue)
        {
            sentences.Enqueue(line);
        }
        Debug.Log("Sentences count: " + sentences.Count);
        DisplayNextSentence();
        StartCoroutine(EnableInputNextFrame());
    }

    private IEnumerator EnableInputNextFrame()
    {
        yield return null;
        inputAllowed = true;
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
        dialogueBox.SetActive(false);

        if (PlayerControls.Instance)
        {
            PlayerControls.Instance.EnableMovement();
        }
        
        OnConversationEnded?.Invoke();
    }


    private void OnContinueInput(InputAction.CallbackContext context)
    {

        if (!gameObject.activeSelf || conversationEnded || !inputAllowed)
            return;

        DisplayNextSentence();
    }


}

