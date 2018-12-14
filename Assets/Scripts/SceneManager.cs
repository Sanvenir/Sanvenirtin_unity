using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using AreaScripts;
using Cinemachine;
using DefaultNamespace;
using ExceptionScripts;
using ObjectScripts;
using ObjectScripts.CharacterController;
using UnityEngine;
using UnityEngine.UI;
using UtilScripts;

public class SceneManager : MonoBehaviour
{

	// Settings
	[Range(1, 100)]
	public int ReactFrames = 1;
	public int CurrentTime;
	public string RandomSeed = "Random";
	public int InitMapX, InitMapY;
	public LayerMask GroundLayer;
	public int LoadingRange = 2;

	// Game Objects
	public EarthMapManager EarthMapManager;
	public GameObject Player;
	public Grid Grid;
	
	[HideInInspector]
	public Dictionary<int, LocalArea> ActivateAreas;
	[HideInInspector]
	public HashSet<int> EdgeIdentities;
	[HideInInspector]
	public LocalArea CenterArea;
	
	[HideInInspector]
	public Character PlayerObject;
	
	// UI Objects
	public Camera MainCamera;
	public GameObject CameraPos;
	public Text GameLogger;
	
	// Script Objects
	public static SceneManager Instance = null;
	[SerializeField] private StringBuilder _loggerText;


	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}

		else if (Instance == this)
		{
			Destroy(gameObject);
		}
		
		if (RandomSeed.Equals("random"))
		{
			RandomSeed = DateTime.Now.ToString(CultureInfo.CurrentCulture);
		}

		ClearLog();

		Utils.ProcessRandom = new System.Random(RandomSeed.GetHashCode());
		
		EarthMapManager.GenerateMap();
		ActivateAreas = new Dictionary<int, LocalArea>();

		var initCoord = new EarthMapCoord(InitMapX, InitMapY);
		CenterArea = GenerateArea(initCoord.GetIdentity(), Vector2Int.zero);
		
		LoadSurroundMap();
	}

	public Vector2 WorldCoordToPos(Vector2Int coord)
	{
		return Grid.GetCellCenterLocal(new Vector3Int(coord.x, coord.y, 0));
	}

	public LocalArea WorldPosToArea(Vector2 pos)
	{
		var hit = Physics2D.OverlapPoint(pos, GroundLayer);
		return hit == null ? null : hit.GetComponent<LocalArea>();
	}

	public Vector2Int WorldPosToCoord(Vector2 pos)
	{
		return Utils.Vector3IntTo2(CenterArea.Tilemap.WorldToCell(pos));
	}
	
	
//	public LocalArea WorldCoordToArea(Vector2Int coord)
//	{
//		foreach (var area in ActivateAreas.Values)
//		{
//			if (area.IsWorldCoordInsideArea(coord))
//			{
//				return area;
//			}
//		}
//
//		throw new AreaNotFoundCondition();
//	}

	public Vector2 NormalizeWorldPos(Vector2 pos)
	{
		var coord = CenterArea.Tilemap.WorldToCell(pos);
		return CenterArea.Tilemap.GetCellCenterWorld(coord);
	}

	public void Print(string message)
	{
		_loggerText.AppendLine(message);
		GameLogger.text = _loggerText.ToString();
	}

	public void ClearLog()
	{
		_loggerText = new StringBuilder();
		GameLogger.text = _loggerText.ToString();
	}
	
	public void LoadSurroundMap()
	{
		// The center area must been initialized;
		// This function loads all uninitialized areas surround the center area
		// Parameters:
		//		x, y: the coordinate of center area on the tile map
		var activateIdentities = new HashSet<int>();
		EdgeIdentities = new HashSet<int>();

		var centerCoord = EarthMapCoord.CreateFromIdentity(CenterArea.Identity);

		for (var dx = -LoadingRange; dx <= LoadingRange; dx++)
		for (var dy = -LoadingRange; dy <= LoadingRange; dy++)
		{
			try
			{
				var identity = centerCoord.GetDeltaCoord(dx, dy).GetIdentity();
				activateIdentities.Add(identity);
				if (dx == LoadingRange || dx == -LoadingRange || 
				    dy == LoadingRange || dy == -LoadingRange)
				{
					EdgeIdentities.Add(identity);
				}
				if (ActivateAreas.ContainsKey(identity))
				{
					continue;
				}
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
		}

		var buffer = new List<int>(ActivateAreas.Keys);
		foreach (var areaIdentity in buffer)
		{
			if (activateIdentities.Contains(areaIdentity))
			{
				continue;
			}
			
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
	void Start ()
	{
		CurrentTime = 0;
		PlayerObject = Player.GetComponent<Character>();
		try
		{
			PlayerObject.Initialize(Vector2Int.zero, CenterArea.Identity);
		}
		catch (CoordOccupiedException e)
		{
			Destroy(e.Collider.gameObject);
		}
		Player.SetActive(true);
	}

	public int GetUpdateTime()
	{
		return Math.Max(1, PlayerObject.GetReactTime() / ReactFrames);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (PlayerObject.IsTurn()) return;
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
