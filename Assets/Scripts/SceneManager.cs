using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using AreaScripts;
using ExceptionScripts;
using MapScripts;
using ObjectScripts.BodyPartScripts;
using ObjectScripts.CharacterController;
using ObjectScripts.CharSubstance;
using ObjectScripts.RaceScripts;
using UIScripts;
using UnityEngine;
using UnityEngine.UI;
using UtilScripts;
using Random = System.Random;

public class SceneManager : MonoBehaviour
{
    // Settings
    [Range(1, 100)] public int ReactFrames = 1;
    public int CurrentTime;
    public string RandomSeed = "Random";
    public int InitMapX, InitMapY;
    public LayerMask GroundLayer;
    public LayerMask ItemLayer;
    public LayerMask BlockLayer;
    public LayerMask PlayerLookAtLayer;
    public LayerMask ObjectLayer;
    public LayerMask BlockInspectLayer;

    public int LoadingRange = 2;

    // Game Objects
    public EarthMapManager EarthMapManager;
    public GameObject Player;
    public Grid Grid;
    public SceneControlButton SceneControlButton;
    public ObjectListMenu ObjectListMenu;
    public ObjectActPanel ObjectActPanel;

    public List<BodyPartList> ComponentList;

    public List<BasicRace> RaceList = new List<BasicRace>
    {
        new BasicRace()
    };

    [HideInInspector] public Dictionary<int, LocalArea> ActivateAreas;
    [HideInInspector] public HashSet<int> EdgeIdentities;
    [HideInInspector] public LocalArea CenterArea;

    [HideInInspector] public Character PlayerObject;

    [HideInInspector] public PlayerController PlayerController;

    // UI Objects
    public Camera MainCamera;
    public GameObject CameraPos;
    public Collider2D SceneCollider;
    public Text GameLogger;
    public BodyPartPanel BodyPartPanel;

    // Script Objects
    public static SceneManager Instance;

    public int MaxLoggerTextLength = 500;
    [SerializeField] private StringBuilder _loggerText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        else if (Instance == this) Destroy(gameObject);

        if (RandomSeed.Equals("random")) RandomSeed = DateTime.Now.ToString(CultureInfo.CurrentCulture);

        ClearLog();

        Utils.ProcessRandom = new Random(RandomSeed.GetHashCode());

        EarthMapManager.GenerateMap();
        ActivateAreas = new Dictionary<int, LocalArea>();

        var initCoord = new EarthMapCoord(InitMapX, InitMapY);
        CenterArea = GenerateArea(initCoord.GetIdentity(), Vector2Int.zero);

        LoadSurroundMap();

        SceneControlButton.enabled = true;

        CurrentTime = 0;
        PlayerObject = Player.GetComponent<Character>();
        PlayerController = Player.GetComponent<PlayerController>();
        Player.SetActive(true);
        try
        {
            PlayerObject.Initialize(Vector2Int.zero, CenterArea.Identity);
        }
        catch (CoordOccupiedException e)
        {
            Destroy(e.Collider.gameObject);
        }

        BodyPartPanel.enabled = true;
    }

    public Vector2Int GetCursorCoord()
    {
        return WorldPosToCoord(SceneControlButton.SceneCursor.transform.position);
    }

    public Vector2 WorldCoordToPos(Vector2Int coord)
    {
        return Grid.GetCellCenterLocal(new Vector3Int(coord.x, coord.y, 0));
    }

    public LocalArea WorldPosToArea(Vector2 pos)
    {
        var hits = new Collider2D[5];
        Physics2D.OverlapPointNonAlloc(pos, hits);
        return (from hit in hits where hit != null select hit.GetComponent<LocalArea>()).FirstOrDefault(area =>
            area != null);
    }

    public LocalArea WorldCoordToArea(Vector2Int coord)
    {
        return WorldPosToArea(WorldCoordToPos(coord));
    }

    public TileType GetTileType(Vector2Int coord)
    {
        var tilemap = WorldCoordToTilemap(coord);
        return tilemap == null ? TileType.None : tilemap.GetTileType(coord);
    }

    public TilemapTerrain WorldPosToTilemap(Vector2 pos)
    {
        var hits = new Collider2D[5];
        Physics2D.OverlapPointNonAlloc(pos, hits, GroundLayer);
        TilemapTerrain upperTilemap = null;
        foreach (var hit in hits)
        {
            if (hit == null) return upperTilemap;
            var tilemap = hit.GetComponent<TilemapTerrain>();
            if (tilemap != null && (upperTilemap == null ||
                                    upperTilemap.TilemapRenderer.sortingOrder < tilemap.TilemapRenderer.sortingOrder))
                upperTilemap = tilemap;
        }

        return upperTilemap;
    }

    public TilemapTerrain WorldCoordToTilemap(Vector2Int coord)
    {
        return WorldPosToTilemap(WorldCoordToPos(coord));
    }

    public Vector2Int WorldPosToCoord(Vector2 pos)
    {
        return Utils.Vector3IntTo2(CenterArea.Tilemap.WorldToCell(pos));
    }


    public Vector2 NormalizeWorldPos(Vector2 pos)
    {
        if (CenterArea == null) return Vector2.zero;

        var coord = CenterArea.Tilemap.WorldToCell(pos);
        return CenterArea.Tilemap.GetCellCenterWorld(coord);
    }

    public void Print(string message, Vector2Int coord)
    {
        if (!PlayerObject.IsAudible(coord)) return;
        _loggerText.AppendLine(message);
        if (_loggerText.Length > MaxLoggerTextLength)
            _loggerText = _loggerText.Remove(0, _loggerText.Length - MaxLoggerTextLength);
        GameLogger.text = _loggerText.ToString();
    }

    public void ClearLog()
    {
        _loggerText = new StringBuilder();
        GameLogger.text = _loggerText.ToString();
    }

    /// <summary>
    ///     The center area must been initialized;
    ///     This function loads all uninitialized areas surround the center area
    /// </summary>
    public void LoadSurroundMap()
    {
        var activateIdentities = new HashSet<int>();
        EdgeIdentities = new HashSet<int>();

        var centerCoord = EarthMapCoord.CreateFromIdentity(CenterArea.Identity);

        for (var dx = -LoadingRange; dx <= LoadingRange; dx++)
        for (var dy = -LoadingRange; dy <= LoadingRange; dy++)
            try
            {
                var identity = centerCoord.GetDeltaCoord(dx, dy).GetIdentity();
                activateIdentities.Add(identity);
                if (dx == LoadingRange || dx == -LoadingRange ||
                    dy == LoadingRange || dy == -LoadingRange)
                    EdgeIdentities.Add(identity);

                if (ActivateAreas.ContainsKey(identity)) continue;

                var area = GenerateArea(
                    identity,
                    CenterArea.WorldStartCoord +
                    new Vector2Int(
                        dx * EarthMapManager.LocalWidth, dy * EarthMapManager.LocalHeight));
            }
            catch (CoordOutOfWorldException)
            {
                Debug.Log("Reach the edge of the global map");
            }

        var buffer = new List<int>(ActivateAreas.Keys);
        foreach (var areaIdentity in buffer)
        {
            if (activateIdentities.Contains(areaIdentity)) continue;

            // Destroy inactivate areas
            Destroy(ActivateAreas[areaIdentity].gameObject);
            ActivateAreas.Remove(areaIdentity);
        }
    }

    private LocalArea GenerateArea(int identity, Vector2Int worldCoord)
    {
        var mapType = EarthMapManager.EarthMap.GetMap(EarthMapCoord.CreateFromIdentity(identity));
        var instance = Instantiate(EarthMapManager.BaseAreaPrefabs[mapType], Grid.transform);
        var area = instance.GetComponent<LocalArea>();
        ActivateAreas.Add(identity, area);
        area.Initialize(identity, worldCoord);
        return area;
    }

    // Use this for initialization
    private void Start()
    {
        PlayerController.UpdateVisual();
    }

    public int GetUpdateTime()
    {
        return Math.Max(1, PlayerObject.Properties.GetReactTime(0) / ReactFrames);
    }

    // Update is called once per frame
    private void Update()
    {
        if (PlayerObject != null && PlayerObject.IsTurn()) return;
        CurrentTime += GetUpdateTime();
    }

    private void LateUpdate()
    {
        var centerIdentity = PlayerObject.AreaIdentity;
        if (!EdgeIdentities.Contains(centerIdentity)) return;
        CenterArea = ActivateAreas[centerIdentity];
        LoadSurroundMap();
    }
}