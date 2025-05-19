using UnityEngine;

public class ColliderChecks : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Magical Thingy"))
        {
            TabletIcon.Instance.VibrateTablet();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Magical Thingy"))
        {
            TabletIcon.Instance.StopVibratingTablet();
        }
    }
}
