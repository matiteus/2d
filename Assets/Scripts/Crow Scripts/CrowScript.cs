using System.Collections;
using UnityEngine;

public class CrowScript : MonoBehaviour
{
    [SerializeField] private Transform crowCry;
    [SerializeField] private AudioClip crowCrySound;
    [SerializeField] private AudioClip crowFlySound;
    private SpriteRenderer crowCrySprite;
    private float flyDistance = 20f;
    private float flySpeed = 8f;
    private Vector2 flyDirection;
    private bool isFlyingAway = false; 
    private AudioSource audioSource;

    private void Awake()
    {
        crowCrySprite = crowCry.GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    public void ActivateCrowCrySprite(Vector2 distanceVector)
    {
        Debug.Log("Crow Cry Position: " + distanceVector);
        crowCry.position = distanceVector; 
        crowCrySprite.enabled = true;
        Debug.Log("Crow cry enabled");
        PlayCrySound();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isFlyingAway && collision.CompareTag("Player"))
        {
            isFlyingAway = true;
            Vector2 playerPosition = collision.transform.position;
            PlayFlyAwaySound();
            StartCoroutine(FlyAwayRoutine(playerPosition));
        }
    }

    private IEnumerator FlyAwayRoutine(Vector2 playerPos)
    {
        float distanceFlown = 0f;
        flyDirection = (transform.position - (Vector3)playerPos).normalized;

        while (distanceFlown < flyDistance)
        {
            float step = flySpeed * Time.deltaTime;
            transform.position += (Vector3)(flyDirection * step);
            distanceFlown += step;
            yield return null;
        }

        Destroy(gameObject);
    }
    private void PlayCrySound()
    {
        if (crowCrySound != null && audioSource != null)
        {
            Debug.Log("Playing crow cry sound!");
            audioSource.PlayOneShot(crowCrySound);
        }
        else
        {
            Debug.LogWarning("Missing AudioSource or Cry sound!");
        }
    }

    private void PlayFlyAwaySound()
    {
        if (crowFlySound != null && audioSource != null)
        {
            Debug.Log("Playing crow fly away sound!");
            audioSource.PlayOneShot(crowFlySound);
        }
        else
        {
            Debug.LogWarning("Missing AudioSource or FlyAway sound!");
        }
    }
}

