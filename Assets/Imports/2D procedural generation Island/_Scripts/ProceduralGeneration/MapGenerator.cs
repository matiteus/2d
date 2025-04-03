using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Directs the map generation logic. Runs All generation steps, and triggeres rendering + collider generation.
/// Sets the offset / Random seed to a specified seed so that we can get repetitive results based on the same
/// seed value
/// </summary>
public class MapGenerator : MonoBehaviour
{

    [SerializeField]
    private GenerationData m_generationData;
    [SerializeField]
    private MapRendering m_mapRendering;
    [SerializeField]
    private TilesCollidersGenerator m_mapColliderGenerator;

    [SerializeField]
    private List<GenerationStep> m_generationSteps;



    [SerializeField]
    private List<RandomWeightedTile> m_weightedRandomTileAssets;
    [SerializeField]
    private List<NoiseDataSO> m_noiseDataToApplySeed;

    public UnityEvent OnFinishedGenerating;

    void Start()
    {
        GenerateMap();
    }

    //For debug purpose
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Mouse0))
    //    {
    //        //get mouse position
    //        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //        Sprite tileSprite = m_mapRendering.GetBaseTilemapSpriteAt(mouseWorldPos, m_generationData);
    //        if (tileSprite != null)
    //        {
    //            Debug.Log($"Tile sprite is: {tileSprite}");
    //        }

    //    }
    //}


    public void GenerateMap()
    {
        //Randomize seed to generate a different map
        if (m_generationData.RandomizeOffset)
        {
            m_generationData.MapGenerationSeed = UnityEngine.Random.Range(0, 1000000);
            foreach (var noiseData in m_noiseDataToApplySeed)
            {
                noiseData.Offset = noiseData.DefaultOffset + new Vector2(m_generationData.MapGenerationSeed, m_generationData.MapGenerationSeed);
            }

        }

        //Sets Seed to the Random Tiles
        foreach (var weightedRandomTile in m_weightedRandomTileAssets)
        {
            weightedRandomTile.SetRandomSeed(m_generationData.MapGenerationSeed);
        }

        //Reset Generation data befor each generation
        m_generationData.ResetData();

        //Run generation steps
        foreach (var generationStep in m_generationSteps)
        {
            if (generationStep != null)
                generationStep.Execute(m_generationData);
        }

        //Visualization
        m_mapRendering.PaintTiles(m_generationData);
        //Generating map colliders
        m_mapColliderGenerator.AddColliders(m_generationData);

        OnFinishedGenerating?.Invoke();
    }



}