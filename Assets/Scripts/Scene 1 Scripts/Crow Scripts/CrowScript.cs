using System.Collections;
using UnityEngine;

public class CrowScript : MonoBehaviour
{
    [SerializeField] private RectTransform crowCry;
    [SerializeField] private AudioClip crowCrySound;
    [SerializeField] private AudioClip crowFlySound;
    [SerializeField] private Transform playerPos;
    [SerializeField] private float cryTimer = 8f;
    [SerializeField] private float crySpriteDuration = 0.5f;
    [SerializeField] private float offset = 20f;
    private float flyDistance = 20f;
    private float flySpeed = 8f;
    private Vector2 flyDirection;
    private bool isFlyingAway = false; 
    private AudioSource audioSource;
    


    private void Awake()
    {
        Debug.Log("crow script awake");
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(CrowCry());
    }

    public void ActivateCrowCrySprite(Vector2 screenPosition)
    {
        crowCry.position = screenPosition;
        crowCry.gameObject.SetActive(true);

        StartCoroutine(DisableCrowCrySprite());
        PlayCrySound();
    }

    private IEnumerator DisableCrowCrySprite()
    {
        yield return new WaitForSeconds(crySpriteDuration);
        crowCry.gameObject.SetActive(false);
        Debug.Log("Crow cry disabled");
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isFlyingAway && collision.CompareTag("Player"))
        {
            isFlyingAway = true;
            Vector2 playerPosition = collision.transform.position;
            PlayFlyAwaySound();
            StopAllCoroutines();
            StartCoroutine(FlyAwayRoutine(playerPosition));
        }
    }
    private IEnumerator CrowCry()
    {
        while (true)
        {
            yield return new WaitForSeconds(cryTimer);

            Vector2 screenPos = Camera.main.WorldToScreenPoint(transform.position);
            bool isOffScreen = (screenPos.x < 0 || screenPos.x > Screen.width || screenPos.y < 0 || screenPos.y > Screen.height);

            if (isOffScreen)
            {
                Vector2 worldEdgePosition = GetBorderIntersection();
                Vector2 screenEdgePosition = Camera.main.WorldToScreenPoint(worldEdgePosition);

                // Clamp to screen edges to ensure the UI stays visible
                screenEdgePosition.x = Mathf.Clamp(screenEdgePosition.x, offset, Screen.width - offset);
                screenEdgePosition.y = Mathf.Clamp(screenEdgePosition.y, offset, Screen.height - offset);

                ActivateCrowCrySprite(screenEdgePosition); // UI version
            }
        }
    }

    private Vector2 GetBorderIntersection()
    {
        // Convert positions to screen space
        Vector2 crowScreenPos = Camera.main.WorldToScreenPoint(transform.position);
        Vector2 playerScreenPos = Camera.main.WorldToScreenPoint(playerPos.position);

        // Direction vector (from crow to player)
        Vector2 direction = (playerScreenPos - crowScreenPos).normalized;

        // Screen bounds
        float left = 0 + offset;
        float right = Screen.width - offset;
        float bottom = 0 + offset;
        float top = Screen.height - offset;

        // Find intersection with each screen edge
        float tLeft = (left - crowScreenPos.x) / direction.x;
        float tRight = (right - crowScreenPos.x) / direction.x;
        float tBottom = (bottom - crowScreenPos.y) / direction.y;
        float tTop = (top - crowScreenPos.y) / direction.y;

        // Find the smallest positive t (valid intersection)
        float t = Mathf.Min(
            tLeft > 0 ? tLeft : float.MaxValue,
            tRight > 0 ? tRight : float.MaxValue,
            tBottom > 0 ? tBottom : float.MaxValue,
            tTop > 0 ? tTop : float.MaxValue
        );

        // Compute intersection point in screen space
        Vector2 screenIntersection = crowScreenPos + direction * t;

        // Convert back to world space
        Vector2 worldIntersection = Camera.main.ScreenToWorldPoint(screenIntersection);

        return worldIntersection;
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

        gameObject.SetActive(false);
    }
    private void PlayCrySound()
    {
        audioSource.PlayOneShot(crowCrySound);
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

