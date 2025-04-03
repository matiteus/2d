using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform playerPos;
    [SerializeField] private Transform crowPos;
    [SerializeField] private Transform cameraPos;
    private CrowScript crow;
    private float cryTimer = 10f;
    private Camera mainCamera;
    private float offset = 20f;

    private void Start()
    {
        crow = crowPos.GetComponent<CrowScript>();
        mainCamera = cameraPos.GetComponent<Camera>();
        StartCoroutine(CrowCry());
    }

    private IEnumerator CrowCry()
    {
        while (true)
        {
            yield return new WaitForSeconds(cryTimer);

            // Convert crow position to screen space
            Vector2 screenPos = mainCamera.WorldToScreenPoint(crowPos.position);
            bool isOffScreen = (screenPos.x < 0 || screenPos.x > Screen.width || screenPos.y < 0 || screenPos.y > Screen.height);

            if (isOffScreen)
            {
                Vector2 cryPosition = GetBorderIntersection();
                crow.ActivateCrowCrySprite(cryPosition);
            }
        }
       
    }

    private Vector2 GetBorderIntersection()
    {
        // Convert positions to screen space
        offset = 10f; // Offset to avoid the edges
        Vector2 crowScreenPos = mainCamera.WorldToScreenPoint(crowPos.position);
        Vector2 playerScreenPos = mainCamera.WorldToScreenPoint(playerPos.position);

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
        Vector2 worldIntersection = mainCamera.ScreenToWorldPoint(screenIntersection);

        return worldIntersection;
    }
}
