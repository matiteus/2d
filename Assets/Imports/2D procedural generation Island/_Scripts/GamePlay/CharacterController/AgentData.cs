using UnityEngine;

public class AgentData : MonoBehaviour
{
    [field: SerializeField]
    public float MaxSpeed { get; private set; } = 2f;

    [field: SerializeField]
    public Vector2 MovementDirection { get; set; }

    [field: SerializeField]
    public string IdleAnimation { get; private set; }
    [field: SerializeField]
    public string MovementAnimationSpeedParameter { get; private set; }
    [field: SerializeField]
    public string MovementAnimationDirectionX { get; private set; }
    [field: SerializeField]
    public string MovementAnimationDirectionY { get; private set; }
    [field: SerializeField]
    public string AttackAnimation { get; private set; }
}
