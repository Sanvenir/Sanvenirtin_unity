using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using AreaScripts;
using DefaultNamespace;
using ExceptionScripts;
using ObjectScripts;
using ObjectScripts.CharacterController;
using UnityEngine;
using UnityEngine.UI;
using UtilScripts;

public class SceneManager : MonoBehaviour
{

	public WorldManager WorldManager;
	public string RandomSeed = "Random";
	public Grid Grid;
	public Text GameLogger;
	
	public int InitWorldX, InitWorldY;

	[Range(1, 100)]
	public int ReactFrames = 1;

	public int CurrentTime;

	[HideInInspector]
	public static SceneManager Instance = null;

	public Dictionary<int, LocalArea> ActivateAreas;
	public HashSet<int> EdgeIdentities;
	public LocalArea CenterArea;

	[SerializeField] private StringBuilder _loggerText;

	public GameObject Player;
	private PlayerController _playerController;
	private Character _playerObject;

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

		_loggerText = new StringBuilder();

		Utils.ProcessRandom = new System.Random(RandomSeed.GetHashCode());
		
		WorldManager.GenerateMap();
		ActivateAreas = new Dictionary<int, LocalArea>();

		var initCoord = new WorldCoord(InitWorldX, InitWorldY);
		CenterArea = GenerateArea(initCoord.GetIdentity(), Vector2Int.zero);
		
		LoadSurroundMap();
	}

	public Vector2 GlobalCoordToPos(Vector2Int coord)
	{
		return Grid.GetCellCenterLocal(new Vector3Int(coord.x, coord.y, 0));
	}

	public LocalArea GlobalCoordToArea(Vector2Int coord)
	{
		foreach (var area in ActivateAreas.Values)
		{
			if (area.IsGlobalCoordInsideArea(coord))
			{
				return area;
			}
		}

		throw new AreaNotFoundCondition();
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

		var centerCoord = WorldCoord.CreateFromIdentity(CenterArea.Identity);

		for (var dx = -2; dx <= 2; dx++)
		for (var dy = -2; dy <= 2; dy++)
		{
			try
			{
				var identity = centerCoord.GetDeltaCoord(dx, dy).GetIdentity();
				activateIdentities.Add(identity);
				if (dx == 2 || dx == -2 || dy == 2 || dy == -2)
				{
					EdgeIdentities.Add(identity);
				}
				if (ActivateAreas.ContainsKey(identity))
				{
					continue;
				}
				var area = GenerateArea(
					identity,
					CenterArea.GlobalStartCoord + 
					new Vector2Int(
						dx * WorldManager.LocalWidth, dy * WorldManager.LocalHeight));
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

	private LocalArea GenerateArea(int identity, Vector2Int globalCoord)
	{
		var mapType = WorldManager.WorldMap.GetMap(WorldCoord.CreateFromIdentity(identity));
		var instance = Instantiate(WorldManager.BaseAreaPrefabs[mapType], Grid.transform);
		var area = instance.GetComponent<LocalArea>();
		ActivateAreas.Add(identity, area);
		area.Initialize(identity, globalCoord);
		return area;
	}

	// Use this for initialization
	void Start ()
	{
		CurrentTime = 0;
		
		_playerController = Player.GetComponent<PlayerController>();
		_playerObject = Player.GetComponent<Character>();
		try
		{
			_playerObject.Initialize(Vector2Int.zero, CenterArea.Identity);
		}
		catch (CoordOccupiedException e)
		{
			Destroy(e.Collider.gameObject);
		}
		Player.SetActive(true);
	}

	public int GetUpdateTime()
	{
		return Math.Max(1, _playerObject.GetReactTime() / ReactFrames);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (_playerController.IsTurn()) return;
		CurrentTime += GetUpdateTime();
	}

	private void LateUpdate()
	{
		var centerIdentity = _playerObject.AreaIdentity;
		if (!EdgeIdentities.Contains(centerIdentity)) return;
		CenterArea = ActivateAreas[centerIdentity];
		LoadSurroundMap();
	}
}
