using UnityEngine;

public class PlayerUIConnector : MonoBehaviour
{
    private PauseMenuController m_pauseMenuController;
    [SerializeField]
    private Agent m_agent;
    [SerializeField]
    private PlayerInput m_playerInput;

    private void Start()
    {
        m_pauseMenuController = FindAnyObjectByType<PauseMenuController>();

        MapGenerator mapGenerator = FindAnyObjectByType<MapGenerator>();
        if (mapGenerator != null)
            m_pauseMenuController.OnRegenerateButtonPressed.AddListener(mapGenerator.GenerateMap);

        m_playerInput = GetComponent<PlayerInput>();
        m_playerInput.OnPauseInput += m_pauseMenuController.ShowPauseMenu;

        m_pauseMenuController.OnPauseMenuToggled.AddListener(m_agent.TogglePause);
    }
}
