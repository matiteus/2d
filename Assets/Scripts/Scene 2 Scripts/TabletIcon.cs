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
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void VibrateTablet()
    {
        if(tabletIcon == null)
        {
            return;
        }
        tabletIcon.sprite = tabletIconOn;
    }
    public void StopVibratingTablet()
    {
        if (tabletIcon == null)
        {
            return;
        }
        tabletIcon.sprite = tabletIconOff;
    }
}
