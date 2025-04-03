using UnityEngine;
using UnityEngine.Events;

public class AnimationEventHelper : MonoBehaviour
{
    public UnityEvent OnWeaponEvent, OnStepEvent;

    public void PlayWeaponEvent()
    {
        OnWeaponEvent?.Invoke();
    }
    public void PlayStepEvent()
    {
        OnStepEvent?.Invoke();
    }
}
