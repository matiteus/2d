using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data shared between the different procedural generation steps
/// </summary>
public class GenerationData : MonoBehaviour
{
    [field: Header("Map Generation Settings")]
    public int MapWidth = 100;

    public int MapHeight = 100;
    /// <summary>
    /// Used to recreate a specific map
    /// </summary>
    public int MapGenerationSeed = 123;

    [SerializeField, Tooltip("Set to true to randomize the map")]
    public bool RandomizeOffset = false;

    /// <summary>
    /// Data for the Base map generation - the first level of our island like ground, water and the sea.
    /// </summary>
    [HideInInspector] public TileType[,] BaseMapTiles = null;
    /// <summary>
    /// Trees needs to be on a separate Tilemap so we will use a separate TileType array for them. We use Array[,] instead
    /// of a HashSet because we have different tree TileTypes (Palm, Hill and default tree)
    /// </summary>
    [HideInInspector] public TileType[,] TreeTiles = null;
    /// <summary>
    /// We are using Rule Tiles https://docs.unity3d.com/Packages/com.unity.2d.tilemap.extras@4.0/manual/RuleTile.html
    /// so sometimes we need to fix some tiles that shows up incorrectly without affecting the Rule tiles output
    /// </summary>
    [HideInInspector] public TileType[,] FixTiles = null;
    /// <summary>
    /// We will add Grass tiles to a separate Tilemap. We use 
    /// </summary>
    [HideInInspector] public HashSet<Vector2Int> GrassPositions = new();
    /// <summary>
    /// Edges of the map will need to be fixed to be DeepSea because of the Deep Sea Rule Tiles (it created shore line there)
    /// </summary>
    [HideInInspector] public HashSet<Vector2Int> FixSeaEdgesPositions = new();
    /// <summary>
    /// We need to gather data about Hill Edges so that we don't place trees / grass there. We want to add "stairs" to the
    /// edges. We also can use it to add colliders only to the edges of the hill.
    /// </summary>
    [HideInInspector] public HashSet<Vector2Int> HillLevel1Edge = new();
    [HideInInspector] public HashSet<Vector2Int> HillLevel2Edge = new();
    /// <summary>
    /// We want to know the interior part of the hills in case we want to place there some special objects.
    /// </summary>
    [HideInInspector] public HashSet<Vector2Int> HillLevel1Interior = new();
    [HideInInspector] public HashSet<Vector2Int> HillLevel2Interior = new();
    /// <summary>
    /// We will place stairs on the edges and exclude those positions from Hill collider generation.
    /// </summary>
    [HideInInspector] public HashSet<Vector2Int> HillStairPositions = new();
    /// <summary>
    /// For prefab placement or placing Player / NPC / Enemies we need to know where we can place them.
    /// This will store all the grass, ground and other tiles where we can walk. No guarantee that it will not be blocked
    /// from all sides by trees (which have a collider).
    /// </summary>
    [HideInInspector] public HashSet<Vector2Int> PossiblePlacementPositions = new();

    /// <summary>
    /// List of all the separate hills on the map
    /// </summary>
    [HideInInspector] public List<HashSet<Vector2Int>> MappedHillsLevel1 = new();
    [HideInInspector] public List<HashSet<Vector2Int>> MappedHillsLevel2 = new();

    /// <summary>
    /// Prefabs placed on the map
    /// </summary>
    [HideInInspector] public List<GameObject> PlacedObjects = new();

    public void ResetData()
    {
        BaseMapTiles = new TileType[MapWidth, MapHeight];
        FixTiles = new TileType[MapWidth, MapHeight];
        TreeTiles = new TileType[MapWidth, MapHeight];

        GrassPositions = new();
        FixSeaEdgesPositions = new();
        HillLevel1Edge = new();
        HillLevel2Edge = new();
        HillLevel1Interior = new();
        HillLevel2Interior = new();
        HillStairPositions = new();
        PossiblePlacementPositions = new();
        MappedHillsLevel1 = new();
        MappedHillsLevel2 = new();

        foreach (var placedObject in PlacedObjects)
        {
            Destroy(placedObject);
        }
        PlacedObjects.Clear();
    }
}

/// <summary>
/// Helper enum where we specify tile types to convert them to TileBase in order to paint the tiles on our Tilemap
/// </summary>
public enum TileType
{
    None,
    Water,
    Sea,
    Ground,
    HillLevel1,
    HillLevel2,
    Sand,
    SandGrass,
    DeepSea,
    Grass,
    GreenTree,
    PalmTree,
    StairsUp,
    StairsDown,
    StairsLeft,
    StairsRight,
    HillTree
}