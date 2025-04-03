using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Helps to move the camera to follow the player
/// </summary>
public class CinemachineCameraHelper : MonoBehaviour
{
    Cinemachine.CinemachineVirtualCamera vcam;

    [SerializeField]
    private float m_cameraDefaultLensSize = 100, m_cameraLensSizeAfterSpawn = 6;
    [SerializeField]
    private Vector3 m_cameraStartPosition = new Vector3(100, 100, -10);
    [SerializeField]
    private float m_delayBeforeZoom = 2, m_lensSizeSmoothTime = 3;
    private float m_currentLensSize;

    public UnityEvent OnStartZoomLogic, OnEndZoomLogic;
    void Awake()
    {
        vcam = FindObjectOfType<Cinemachine.CinemachineVirtualCamera>();
    }

    private void Start()
    {
        if (vcam != null)
        {
            vcam.m_Lens.OrthographicSize = m_cameraDefaultLensSize;
            vcam.transform.position = m_cameraStartPosition;
            OnStartZoomLogic?.Invoke();
            StartCoroutine(ZoomToPlayer());
        }
    }

    private IEnumerator ZoomToPlayer()
    {
        yield return new WaitForSeconds(m_delayBeforeZoom);

        float elapsedTime = 0;
        float startLensSize = m_cameraDefaultLensSize;
        float targetLensSize = m_cameraLensSizeAfterSpawn;

        Vector3 cameraStartPosition = vcam.transform.position;
        Vector3 cameraEndPosition = transform.position;
        cameraEndPosition.z = cameraStartPosition.z;

        while (elapsedTime < m_lensSizeSmoothTime)
        {
            m_currentLensSize = Mathf.Lerp(startLensSize, targetLensSize, elapsedTime / m_lensSizeSmoothTime);
            vcam.m_Lens.OrthographicSize = m_currentLensSize;
            vcam.transform.position = Vector3.Lerp(cameraStartPosition, cameraEndPosition, elapsedTime / m_lensSizeSmoothTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        vcam.Follow = transform;
        vcam.PreviousStateIsValid = false;
        OnEndZoomLogic?.Invoke();
        vcam.m_Lens.OrthographicSize = targetLensSize;

    }

    private void OnDisable()
    {
        if (vcam == null) return;
        vcam.m_Lens.OrthographicSize = m_cameraDefaultLensSize;
        vcam.Follow = null;
        vcam.LookAt = null;
    }
}
