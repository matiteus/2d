using UnityEngine;
using UnityEngine.UI;


public class TabletIcon : MonoBehaviour
{
   public static TabletIcon Instance { get; private set; }
    [SerializeField] Sprite tabletIconOff;
    [SerializeField] Sprite tabletIconOn;
    [SerializeField] Image tabletIcon;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void VibrateTablet()
    {
        tabletIcon.sprite = tabletIconOn;
    }
    public void StopVibratingTablet()
    {
        tabletIcon.sprite = tabletIconOff;
    }
}
