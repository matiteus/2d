using UnityEngine;

public class CompassPuzzleManager : MonoBehaviour
{
    [SerializeField] private GameObject compassPuzzlePrefab;
    [SerializeField] private Transform compassPuzzleLocation;



    private void Start()
    {
        GameObject compassPuzzle = Instantiate(compassPuzzlePrefab, compassPuzzleLocation.position, Quaternion.identity);
    }
}
