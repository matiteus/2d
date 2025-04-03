using UnityEngine;

[CreateAssetMenu]
public class NoiseDataSO : ScriptableObject
{
    public float Scale = 0.03f;
    public int Octaves = 3;
    public float Persistence = 7;
    public float Lacunarity = 0.01f;

    public Vector2 Offset = Vector2.zero;
    public Vector2 DefaultOffset = Vector2.zero;
}
