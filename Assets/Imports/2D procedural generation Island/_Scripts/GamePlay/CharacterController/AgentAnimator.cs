using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AgentAnimator : MonoBehaviour
{
    [SerializeField]
    private Animator m_animator;


    public event Action OnAnimationEndEvent;

    private void Awake()
    {
        if (m_animator == null)
            m_animator = GetComponent<Animator>();
    }

    public void SetMovementAnimationSpeed(Vector2 velocity, string speedAnimationParameter)
    {
        m_animator.SetFloat(speedAnimationParameter, velocity.magnitude);
    }

    public void PlayAnimation(string movementAnimationName)
    {
        m_animator.Play(movementAnimationName);
    }

    public void PerformAnimationEndEvent()
    {
        OnAnimationEndEvent?.Invoke();
        OnAnimationEndEvent = null;
    }

    internal void SetAnimationDirection(Vector2 movementInput, string directionXparameter, string directionYparameter)
    {
        m_animator.SetFloat(directionXparameter, movementInput.x);
        m_animator.SetFloat(directionYparameter, movementInput.y);
    }
}
