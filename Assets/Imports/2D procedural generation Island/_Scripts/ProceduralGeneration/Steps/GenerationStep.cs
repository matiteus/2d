using UnityEngine;

public abstract class GenerationStep : MonoBehaviour
{
    [SerializeField, TextArea(15, 20)]
    private string m_description;
    public abstract void Execute(GenerationData generationData);
}
