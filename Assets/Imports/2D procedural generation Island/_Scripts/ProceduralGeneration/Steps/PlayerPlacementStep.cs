using UnityEngine;

public class PlayerPlacementStep : GenerationStep
{
    [SerializeField]
    private bool m_applyStep = true;
    /// <summary>
    /// We will use a Random number generator for prefab placement.
    /// </summary>
    private System.Random m_prefabPlacementRandom;

    [SerializeField]
    private GameObject m_playerPrefab;

    /// <summary>
    /// Here we place the player prefab on the map.
    /// </summary>
    /// <param name="generationData"></param>
    public override void Execute(GenerationData generationData)
    {
        if (m_applyStep == false)
            return;

        //To ensure that the placement is repetitive we set the seed of the generator to a known value
        m_prefabPlacementRandom = new System.Random(generationData.MapGenerationSeed);

        int randomIndex = m_prefabPlacementRandom.Next(generationData.PossiblePlacementPositions.Count);
        int index = 0;

        //This is how we could randomly place any prefab (not only a player)
        foreach (var position in generationData.PossiblePlacementPositions)
        {
            //Because our PossiblePlacementPositions is a Hashset we need to loop to find the random position
            if (index == randomIndex)
            {
                //Here if we have an object that needs to be placed on multiple tiles we would add some check
                //to ensure that we can place it on all of them (that there are no collisions)
                //AAlternatively we could clear the terrain if there are trees or other objects in the way
                //that can be removed (so not hills or water tiles).
                GameObject playerReference
                    = Instantiate(m_playerPrefab, new(position.x, position.y, 0), Quaternion.identity);
                generationData.PlacedObjects.Add(playerReference);
                break;
            }
            index++;
        }
    }
}
