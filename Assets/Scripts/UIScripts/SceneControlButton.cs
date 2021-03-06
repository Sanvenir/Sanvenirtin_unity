using ObjectScripts;
using ObjectScripts.CharacterController;
using ObjectScripts.CharacterController.PlayerOrder;
using ObjectScripts.CharSubstance;
using UnityEngine;
using UnityEngine.EventSystems;
using UtilScripts;

namespace UIScripts
{
//    [CustomEditor(typeof(SceneControlButton))]
//    public class SceneControlButtonEditor : Editor
//    {
//        public override void OnInspectorGUI()
//        {
//            base.OnInspectorGUI();
//        }
//    }
    public class SceneControlButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private readonly Collider2D[] _hits = new Collider2D[1];

        private Vector3 _cursorTargetPos;
        private Collider2D _selected;
        private int _selectedOriginLayer;

        [HideInInspector] public bool CameraFollowPlayer;

        public Collider2D CursorCollider;
        [HideInInspector] public Character Player;
        [HideInInspector] public PlayerController PlayerController;
        public SpriteRenderer SceneCursor;
        public ContactFilter2D SceneCursorFilter;
        public ActionMenu SelectionMenu;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (SceneCursor.enabled) return;
            SceneCursor.enabled = true;
            _cursorTargetPos = SceneManager.Instance.MainCamera
                .ScreenToWorldPoint(Input.mousePosition);
            SceneCursor.transform.position =
                SceneManager.Instance.NormalizeWorldPos(_cursorTargetPos);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (Input.GetMouseButton(0) || Input.GetMouseButton(1)) return;

            if (SceneCursor == null) return;
            SceneCursor.enabled = false;
        }

        protected void Awake()
        {
            CursorCollider = SceneCursor.GetComponent<Collider2D>();
        }

        protected void Start()
        {
            Player = SceneManager.Instance.PlayerObject;
            PlayerController = SceneManager.Instance.Player.GetComponent<PlayerController>();
            CameraFollowPlayer = true;
        }

        private void CancelSelected()
        {
            if (_selected == null) return;
            var substance = _selected.GetComponent<Substance>();
            if (substance == null) return;
            substance.Selected = false;
        }

        private void ChangeSelected()
        {
            if (_selected == _hits[0]) return;

            if (_selected != null) CancelSelected();

            _selected = _hits[0];
            if (_selected == null) return;
            var substance = _selected.GetComponent<Substance>();
            if (substance == null) return;
            substance.Selected = true;
        }

        private void Update()
        {
            if (Player != null) SceneManager.Instance.SceneCollider.transform.position = Player.GetVisualPos();


            // Soft move the camera according to Scene Cursor or Player
            if (CameraFollowPlayer && Player != null)
                SceneManager.Instance.CameraPos.transform.position =
                    SceneManager.Instance.CameraPos.transform.position * 0.9f +
                    Player.transform.position * 0.1f;
            else
                SceneManager.Instance.CameraPos.transform.position =
                    SceneManager.Instance.CameraPos.transform.position * 0.9f +
                    SceneCursor.transform.position * 0.1f;

            // If SceneCursor is disabled, do not update it
            if (!SceneCursor.enabled)
            {
                GameManager.Instance.GameCursor.SetActive(true);
                CameraFollowPlayer = true;
                if (!SelectionMenu.enabled) return;
                SelectionMenu.EndUp();
                return;
            }


            GameManager.Instance.GameCursor.SetActive(false);
            var targetPos = SceneManager.Instance.NormalizeWorldPos(_cursorTargetPos);


            SceneCursor.transform.position =
                Utils.Vector3To2(SceneCursor.transform.position * 0.5f) +
                targetPos * 0.5f;

            // If a collider is chosen, highlight it; 
            _hits[0] = null;
            Physics2D.OverlapPoint(SceneCursor.transform.position, SceneCursorFilter, _hits);
            ChangeSelected();

            // Control

            // If Right Mouse Button Holding, Scene Cursor no longer be updated
            if (!Input.GetMouseButton(1))
            {
                // Soft move the Scene Cursor
                // Left Mouse Button Holding, Camera following the cursor
                // Else Camera following the player
                if (Input.GetMouseButton(0))
                {
                    CameraFollowPlayer = false;
                    _cursorTargetPos.x += Input.GetAxis("Mouse X") * 2;
                    _cursorTargetPos.y += Input.GetAxis("Mouse Y") * 2;
                }
                else
                {
                    CameraFollowPlayer = true;
                    var mousePos = SceneManager.Instance.MainCamera
                        .ScreenToWorldPoint(Input.mousePosition);
                    _cursorTargetPos = mousePos;
                }
            }

            // Right Mouse Button Down, Show the order list
            if (Input.GetMouseButtonDown(1))
            {
                var worldCoord = SceneManager.Instance.WorldPosToCoord(
                    SceneCursor.transform.position);
                var direction = Utils.VectorToDirection(worldCoord - Player.WorldCoord);

                PlayerController.TargetCharacter = _selected == null ? null : _selected.GetComponent<Character>();
                PlayerController.TargetCoord = worldCoord;
                PlayerController.TargetDirection = direction;

                SelectionMenu.StartUp(targetPos);
                return;
            }

            // Right Mouse Button Up, return the chosen order
            if (Input.GetMouseButton(1)) return;
            if (!SelectionMenu.enabled) return;
            var order = SelectionMenu.EndUp();
            PlayerController.CurrentOrder = order as BaseOrder;
        }
    }
}