using UnityEngine;

public class StepSound : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] stepSounds;
    [SerializeField]
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayStepSound()
    {
        audioSource.clip = stepSounds[Random.Range(0, stepSounds.Length)];
        audioSource.pitch = Random.Range(0.8f, 1.2f); // Randomize pitch
        audioSource.Play();
    }
}

