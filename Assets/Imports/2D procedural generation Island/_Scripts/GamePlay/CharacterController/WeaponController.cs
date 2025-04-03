using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField]
    private AudioSource m_attackSound;
    [SerializeField]
    private TreeRemover m_treeRemover;
    [SerializeField]
    private AgentData m_agentData;
    private void Awake()
    {
        m_treeRemover = FindAnyObjectByType<TreeRemover>();
    }
    public void PerformAttack()
    {
        m_attackSound.Play();
        m_treeRemover.TryRemovingTreeAt(transform.position + (Vector3)m_agentData.MovementDirection.normalized * 0.5f);
    }

}
