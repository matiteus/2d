using UnityEngine;

public class AgentMover : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D m_rigidbody2D;

    public Vector2 Velocity => m_rigidbody2D.linearVelocity;

    public void Move(Vector2 movementDirection, float maxSpeed)
    {
        m_rigidbody2D.linearVelocity = movementDirection * maxSpeed;
    }

}
