using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject GameCursor;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        else if (Instance == this) Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        Application.targetFrameRate = 60;
    }


    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        Cursor.visible = false;
        GameCursor.transform.position = Input.mousePosition;
    }
}