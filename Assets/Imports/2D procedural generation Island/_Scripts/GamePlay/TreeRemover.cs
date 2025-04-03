using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TreeRemover : MonoBehaviour
{
    [SerializeField]
    private Tilemap m_treeTilemap;
    [SerializeField]
    private GameObject m_treeHitParticle;
    [SerializeField]
    private AudioSource m_TreeHitSound;

    public void TryRemovingTreeAt(Vector2 position)
    {
        Vector3Int treeTilePosition = m_treeTilemap.WorldToCell(position);
        if (m_treeTilemap.HasTile(treeTilePosition))
        {

            Instantiate(m_treeHitParticle, treeTilePosition + new Vector3(0.5f, 0.5f), Quaternion.identity);
            m_TreeHitSound.Play();
            StopAllCoroutines();
            StartCoroutine(WaitBeforeRemovingTree(treeTilePosition, m_TreeHitSound.clip.length / 2f));
        }
    }

    private IEnumerator WaitBeforeRemovingTree(Vector3Int treeTilePosition, float delay)
    {
        yield return new WaitForSeconds(delay);
        m_treeTilemap.SetTile(treeTilePosition, null);
    }
}
