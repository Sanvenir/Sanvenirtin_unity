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
    public class SceneControlButton: Button
    {
        public ActionMenu ActionMenu;
        public SpriteRenderer SceneCursor;
        private Vector3 _cursorTargetPos;
        private bool _updatingCursor;

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
            _updatingCursor = true;
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            _updatingCursor = false;
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
                            new RestOrder("Rest", null, direction, worldCoord),
                            new WalkToOrder("Walk To", null, direction, worldCoord), 
                            new AttackDirectionOrder("Attack Direction", null, direction, worldCoord)
                        };
                
                        var hit = Physics2D.OverlapPoint(mousePos);
                        if (hit == null) return;
                        var targetCharacter = hit.GetComponent<Character>();
                        if (targetCharacter != null)
                        {
                            if (!targetCharacter.Dead)
                            {
                                orderList.Add(new KillCharacterOrder("Kill Character", targetCharacter, direction, worldCoord));
                            }
                        }
                
                        ActionMenu.StartUp(worldCoord, orderList);
                        break;
            }
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            var order = ActionMenu.EndUp();
            PlayerController.CurrentOrder = order;
            _updatingCursor = true;
        }

        private void Update()
        {
            SceneCursor.transform.position = 
                SceneCursor.transform.position * 0.5f + _cursorTargetPos * 0.5f;
            if (!_updatingCursor) return;
            var mousePos = SceneManager.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition);
            _cursorTargetPos = SceneManager.Instance.NormalizeWorldPos(mousePos);

        }
    }
}