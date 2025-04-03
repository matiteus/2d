using System.Collections;
using UnityEngine;

public class CrowScript : MonoBehaviour
{
    private float cryTimer = 1.5f;
    [SerializeField] private Transform crowCry;
    private SpriteRenderer crowCrySprite;
    private float flyDistance = 20f;
    private float flySpeed = 8f;
    private Vector2 flyDirection;
    private bool isFlyingAway = false; // Prevents multiple flight triggers

    private void Awake()
    {
        crowCrySprite = crowCry.GetComponent<SpriteRenderer>();
    }

    public void ActivateCrowCrySprite(Vector2 distanceVector)
    {
        Debug.Log("Crow Cry Position: " + distanceVector);
        crowCry.position = distanceVector; // Set the cry position
        crowCrySprite.enabled = true;
        Debug.Log("Crow cry enabled");
        StartCoroutine(DisableCrySprite());
    }

    private IEnumerator DisableCrySprite()
    {
        yield return new WaitForSeconds(cryTimer);
        crowCrySprite.enabled = false;
        Debug.Log("Crow cry disabled");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isFlyingAway && collision.CompareTag("Player"))
        {
            isFlyingAway = true; // Prevents multiple activations
            Vector2 playerPosition = collision.transform.position;
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

        gameObject.SetActive(false);
    }
}
