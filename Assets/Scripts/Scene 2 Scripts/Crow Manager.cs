using UnityEngine;

public class CrowManager : MonoBehaviour
{
    [SerializeField] private GameObject TabletCrow;
    [SerializeField] private GameObject letterCrow;
    [SerializeField] private GameObject compassCrow;
    

    public static CrowManager Instance { get; private set; }

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

    private void Start()
    {
        if(!TabletCrow)
        {   //it means that its the second scene
            if (SceneLoader.Instance.GetHasSeenLetter())
            {
                if (SceneLoader.Instance.GetHasFinishedCompassPuzzle())
                {
                    letterCrow.SetActive(false);
                    compassCrow.SetActive(false);
                }
                else
                {
                    letterCrow.SetActive(false);
                    compassCrow.SetActive(true);
                }
            }
            else
            {
                letterCrow.SetActive(true);
                compassCrow.SetActive(false);
            }
        }
        else
        {
            //first scene
            if(SceneLoader.Instance.GetHasTablet())
            {
                TabletCrow.SetActive(false);
            }
            else
            {
                TabletCrow.SetActive(true);
            }

        }
        
    }

    public void ActivateCompassCrow()
    {
        compassCrow.SetActive(true);
    }
}
