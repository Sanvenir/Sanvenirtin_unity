using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public GameObject GameCursor;

	public static GameManager Instance = null;

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

		DontDestroyOnLoad(gameObject);
	}


	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update () {
		Cursor.visible = false;
		GameCursor.transform.position = Input.mousePosition;
	}
}
