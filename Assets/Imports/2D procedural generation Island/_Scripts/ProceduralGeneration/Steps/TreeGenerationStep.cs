using UnityEngine;

public class TreeGenerationStep : GenerationStep
{
    [SerializeField] private bool m_applyTrees = true;

    [SerializeField] private NoiseDataSO m_treeNoiseData;
    [SerializeField] private NoiseDataSO m_palmTreeData;

    [SerializeField, Range(0, 1), Tooltip("Higher the value the more green trees will be added")] private float m_treeAmount = 0.5f;
    [SerializeField, Range(0, 1), Tooltip("Higher the value the more palm trees will be added")] private float m_palmTreeAmount = 0.315f;

    public override void Execute(GenerationData generationData)
    {
        if (m_applyTrees)
        {
            float[,] treeMap = NoseGenerationHelper.GeneratePerlinNoiseMap(generationData.MapWidth, generationData.MapHeight, m_treeNoiseData);
            float[,] palmTreeMap = NoseGenerationHelper.GeneratePerlinNoiseMap(generationData.MapWidth, generationData.MapHeight, m_palmTreeData);
            for (int x = 0; x < generationData.MapWidth; x++)
            {
                for (int y = 0; y < generationData.MapHeight; y++)
                {
                    //Check if the tile is an edge tile of the hill. We dont want to place tree or grass
                    //on the edge of the hill
                    bool isHillEdgeTile = generationData.HillLevel1Edge.Contains(new(x, y))
                        || generationData.HillLevel2Edge.Contains(new(x, y));

                    if (treeMap[x, y] > (1 - m_treeAmount) &&
                        (generationData.BaseMapTiles[x, y] == TileType.Ground || generationData.BaseMapTiles[x, y] == TileType.HillLevel1) && isHillEdgeTile == false)
                    {
                        //We place different trees on the level 1 hill and on the ground
                        if (generationData.HillLevel1Interior.Contains(new(x, y)))
                        {
                            generationData.TreeTiles[x, y] = TileType.HillTree;
                        }
                        else
                        {
                            generationData.TreeTiles[x, y] = TileType.GreenTree;
                        }
                    }
                    //We place palm trees on the sand / sand grass tiles
                    if (palmTreeMap[x, y] > (1 - m_palmTreeAmount) && (generationData.BaseMapTiles[x, y] == TileType.Sand || generationData.BaseMapTiles[x, y] == TileType.SandGrass))
                    {
                        generationData.TreeTiles[x, y] = TileType.PalmTree;
                    }
                }
            }
        }
    }

}
