using UnityEngine;

public class ScriptforCompassCutscene : MonoBehaviour
{
    [SerializeField] private CompassManager compassManager;
    private void Awake()
    {
        compassManager.ActivateAllTorches();
    }

}
