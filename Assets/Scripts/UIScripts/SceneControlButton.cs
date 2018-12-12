using System.Collections;
using System.Collections.Generic;
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

        public Character Player;
        public PlayerController PlayerController;

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
            var mousePos = SceneManager.Instance.MainCamera.ScreenToWorldPoint(eventData.position);
            var worldPos = SceneManager.Instance.NormalizeWorldPos(mousePos);
            var worldCoord = SceneManager.Instance.WorldPosToCoord(mousePos);
            var direction = Utils.VectorToDirection(worldCoord - Player.WorldCoord);
            switch (eventData.button)
            {
                case PointerEventData.InputButton.Left:
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

                    var hit = Physics2D.OverlapPoint(mousePos);
                    if (hit == null) return;
                    var targetCharacter = hit.GetComponent<Character>();
                    if (targetCharacter != null && targetCharacter != Player)
                    {
                        if (!targetCharacter.Dead)
                        {
                            orderList.Add(new KillCharacterOrder("Kill", targetCharacter, direction, worldCoord));
                        }
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

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            SceneCursor.enabled = true;
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            SceneCursor.enabled = false;
        }

        private void FixedUpdate()
        {
            SceneCursor.transform.position =
                SceneCursor.transform.position * 0.5f + _cursorTargetPos * 0.5f;
            if (Input.GetMouseButton(0) || Input.GetMouseButton(1)) return;
            var mousePos = SceneManager.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition);
            _cursorTargetPos = SceneManager.Instance.NormalizeWorldPos(mousePos);
            var hit = Physics2D.OverlapPoint(mousePos);
            if (hit != null)
            {
                // TODO: Add Bloom Effect
            }
        }
    }
}