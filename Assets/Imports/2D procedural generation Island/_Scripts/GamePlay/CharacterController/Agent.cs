using UnityEngine;

[RequireComponent(typeof(AgentData))]
public class Agent : MonoBehaviour
{
    private IInput m_input;
    [SerializeField]
    private AgentMover m_agentMover;
    [SerializeField]
    private AgentAnimator m_agentAnimator;
    private AgentData m_agentData;

    [field: SerializeField]
    public bool IsPaused { get; private set; }


    private void Awake()
    {
        IsPaused = false;
        m_input = GetComponent<IInput>();
        Debug.Assert(m_input != null, "No IInput component found on Agent");
        if (m_input != null)
            m_input.OnActionInput += HandleAction;

        m_agentData = GetComponent<AgentData>();
    }



    public void TogglePause(bool isPaused)
    {
        IsPaused = isPaused;
    }

    private void HandleAction()
    {
        m_agentMover.Move(Vector2.zero, m_agentData.MaxSpeed);
        IsPaused = true;
        m_agentAnimator.PlayAnimation(m_agentData.AttackAnimation);
        m_agentAnimator.OnAnimationEndEvent +=
            () => IsPaused = false;
        m_agentAnimator.OnAnimationEndEvent +=
            () => m_agentAnimator.PlayAnimation(m_agentData.IdleAnimation);
    }

    private void Update()
    {
        if (IsPaused)
            return;

        if (m_input.MovementInput.magnitude > 0)
        {
            m_agentData.MovementDirection = m_input.MovementInput;
            m_agentAnimator.SetAnimationDirection(m_input.MovementInput,
                m_agentData.MovementAnimationDirectionX, m_agentData.MovementAnimationDirectionY);
        }

        m_agentMover.Move(m_input.MovementInput.normalized, m_agentData.MaxSpeed);

        m_agentAnimator.SetMovementAnimationSpeed(m_input.MovementInput,
            m_agentData.MovementAnimationSpeedParameter);
    }
}
