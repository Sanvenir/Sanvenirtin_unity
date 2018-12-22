using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using ObjectScripts;
using ObjectScripts.CharacterController;
using ObjectScripts.CharacterController.PlayerOrder;
using ObjectScripts.CharSubstance;
using SpriteGlow;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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
    public class SceneControlButton : Button
    {
        public ActionMenu ActionMenu;
        public SpriteRenderer SceneCursor;

        private Vector3 _cursorTargetPos;
        [HideInInspector] public Character Player;
        [HideInInspector] public PlayerController PlayerController;

        public bool CameraFollowPlayer;

        private Collider2D _cursorCollider;
        private readonly Collider2D[] _hits = new Collider2D[1];
        private Collider2D _selected;
        private int _selectedOriginLayer;

        protected override void Awake()
        {
            base.Awake();
            ActionMenu = GetComponent<ActionMenu>();
            _cursorCollider = SceneCursor.GetComponent<Collider2D>();
        }

        protected override void Start()
        {
            base.Start();
            Player = SceneManager.Instance.PlayerObject;
            PlayerController = SceneManager.Instance.Player.GetComponent<PlayerController>();
            CameraFollowPlayer = true;
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            SceneCursor.enabled = true;
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
            {
                return;
            }

            SceneCursor.enabled = false;
        }

        private void CancelSelected()
        {
            if (_selected == null) return;
            var effect = _selected.GetComponent<SpriteRenderer>();
            if (effect == null) return;
            effect.sortingLayerID = _selectedOriginLayer;
            effect.color = Color.white;
            _selected = null;
        }

        private void ChangeSelected()
        {
            if (_selected == _hits[0])
            {
                return;
            }

            if (_selected != null)
            {
                CancelSelected();
            } 
            
            _selected = _hits[0];
            if (_selected == null) return;
            var effect = _selected.GetComponent<SpriteRenderer>();
            if (effect == null) return;
            _selectedOriginLayer = effect.sortingLayerID;
            effect.sortingLayerName = "OnTop";
            effect.color = Color.gray;
        }

        private void Update()
        {
            if (Player != null)
            {
                SceneManager.Instance.SceneCollider.transform.position = Player.transform.position;
            }

            // Soft move the camera according to Scene Cursor or Player
            if (CameraFollowPlayer && Player != null)
            {
                SceneManager.Instance.CameraPos.transform.position =
                    SceneManager.Instance.CameraPos.transform.position * 0.9f +
                    Player.transform.position * 0.1f;
            }
            else
            {
                SceneManager.Instance.CameraPos.transform.position =
                    SceneManager.Instance.CameraPos.transform.position * 0.9f +
                    SceneCursor.transform.position * 0.1f;
            }
                        
            // If SceneCursor is disabled, do not update it
            if (!SceneCursor.enabled)
            {
                GameManager.Instance.GameCursor.SetActive(true);
                CameraFollowPlayer = true;
                if (!ActionMenu.enabled) return;
                ActionMenu.EndUp();
                return;
            }

            GameManager.Instance.GameCursor.SetActive(false);
            var targetPos = SceneManager.Instance.NormalizeWorldPos(_cursorTargetPos);


            SceneCursor.transform.position =
                Utils.Vector2To3(SceneCursor.transform.position * 0.5f) +
                targetPos * 0.5f;

            // If a collider is chosen, highlight it; 
            if ( _cursorCollider.OverlapCollider(
                     SceneManager.Instance.BlockFilter, _hits) != 0)
            {
                ChangeSelected();
            }
            else
            {
                CancelSelected();
            }

            
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
                
                var orderList = new List<BaseOrder>
                {
                    new RestOrder("Rest", null, direction, worldCoord)
                };
                if (direction != Direction.None)
                {
                    orderList.Insert(
                        0, new WalkToOrder("Walk To", null, direction, worldCoord));
                    orderList.Add(
                        new AttackDirectionOrder("Attack", null, direction, worldCoord));
                }
                
                ActionMenu.StartUp(targetPos, orderList);
                return;
            }

            // Right Mouse Button Up, return the chosen order
            if (Input.GetMouseButton(1)) return;
            if (!ActionMenu.enabled) return;
            var order = ActionMenu.EndUp();
            PlayerController.CurrentOrder = order;
 

        }
    }
}