using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class PauseMenuController : MonoBehaviour
{

    Button m_generateButton;
    VisualElement root;

    public UnityEvent<bool> OnPauseMenuToggled;
    public UnityEvent OnRegenerateButtonPressed;

    private void Awake()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        root.style.display = DisplayStyle.None;
        m_generateButton = root.Q<Button>("GenerateBtn");
        m_generateButton.clicked += HandleGenerateButton;
    }

    private void HandleGenerateButton()
    {
        m_generateButton.text = "Regenerating...";

        StartCoroutine(TriggerRegenerationLogic());
    }

    private IEnumerator TriggerRegenerationLogic()
    {
        yield return new WaitForSeconds(1f);
        OnRegenerateButtonPressed?.Invoke();
        root.style.display = DisplayStyle.None;
    }

    public void ShowPauseMenu()
    {
        bool isPauseMenuVisible = root.style.display == DisplayStyle.Flex;
        if (isPauseMenuVisible == false)
        {
            m_generateButton.text = "Generate Map";
        }
        root.style.display = isPauseMenuVisible ? DisplayStyle.None : DisplayStyle.Flex;

        OnPauseMenuToggled?.Invoke(!isPauseMenuVisible);
    }
}
