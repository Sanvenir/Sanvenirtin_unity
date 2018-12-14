using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using ObjectScripts;
using ObjectScripts.CharacterController;
using ObjectScripts.CharacterController.PlayerOrder;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UtilScripts;

namespace UIScripts
{
    public class SceneControlButton : Button
    {
        public ActionMenu ActionMenu;
        public SpriteRenderer SceneCursor;
        private Vector3 _cursorTargetPos;

        [HideInInspector] public Character Player;
        [HideInInspector] public PlayerController PlayerController;

        public bool CameraFollowPlayer = true;

        protected override void Awake()
        {
            base.Awake();
            ActionMenu = GetComponent<ActionMenu>();
            SceneCursor = GetComponentInChildren<SpriteRenderer>();
        }

        protected override void Start()
        {
            base.Start();
            Player = SceneManager.Instance.PlayerObject;
            PlayerController = SceneManager.Instance.Player.GetComponent<PlayerController>();
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            var worldCoord = SceneManager.Instance.WorldPosToCoord(
                SceneCursor.transform.position);
            var direction = Utils.VectorToDirection(worldCoord - Player.WorldCoord);
            switch (eventData.button)
            {
                case PointerEventData.InputButton.Left:
                    CameraFollowPlayer = false;
                    break;
                case PointerEventData.InputButton.Right:
                    var orderList = new List<BaseOrder>
                    {
                        new RestOrder("Rest", null, direction, worldCoord)
                    };
                    if (direction != Direction.None)
                    {
                        orderList.Add(
                            new AttackDirectionOrder("Attack", null, direction, worldCoord));

                        orderList.Add(
                            new WalkToOrder("Walk To", null, direction, worldCoord));
                    }

                    var hit = Physics2D.OverlapPoint(SceneCursor.transform.position);
                    if (hit == null) return;
                    var targetCharacter = hit.GetComponent<Character>();
                    if (targetCharacter != null && targetCharacter != Player)
                    {
                    }

                    ActionMenu.StartUp(worldCoord, orderList);
                    break;
            }
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            if (!ActionMenu.enabled) return;
            var order = ActionMenu.EndUp();
            PlayerController.CurrentOrder = order;
        }
//
//        public override void OnPointerEnter(PointerEventData eventData)
//        {
//            base.OnPointerEnter(eventData);
//            SceneCursor.enabled = true;
//        }
//
//        public override void OnPointerExit(PointerEventData eventData)
//        {
//            base.OnPointerExit(eventData);
//            SceneCursor.enabled = false;
//        }

        private void FixedUpdate()
        {
            if (Input.GetMouseButton(1)) return;
            if (Input.GetMouseButton(0) && !CameraFollowPlayer)
            {
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
//
//            _cursorTargetPos.x = Mathf.Min(
//                Player.transform.position.x + EarthMapManager.LocalWidth, _cursorTargetPos.x);
//            _cursorTargetPos.x = Mathf.Max(
//                Player.transform.position.x - EarthMapManager.LocalWidth, _cursorTargetPos.x);
//            _cursorTargetPos.y = Mathf.Min(
//                Player.transform.position.y + EarthMapManager.LocalHeight, _cursorTargetPos.y);
//            _cursorTargetPos.y = Mathf.Max(
//                Player.transform.position.y - EarthMapManager.LocalHeight, _cursorTargetPos.y);
//            
            SceneCursor.transform.position =
                Utils.Vector2To3(SceneCursor.transform.position * 0.5f) +
                SceneManager.Instance.NormalizeWorldPos(_cursorTargetPos) * 0.5f;

            if (CameraFollowPlayer)
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

            var hit = Physics2D.OverlapPoint(SceneCursor.transform.position);
            if (hit != null)
            {
                // TODO: Add Bloom Effect
            }
        }
    }
}