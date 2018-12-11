using System.Collections;
using System.Collections.Generic;
using ObjectScripts;
using ObjectScripts.CharacterController;
using ObjectScripts.CharacterController.PlayerOrder;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UtilScripts;

namespace UIScripts
{
    public class SceneControlButton: Button
    {
        public ActionMenu ActionMenu;

        public Character Player;
        public PlayerController PlayerController;

        protected override void Awake()
        {
            base.Awake();
            ActionMenu = GetComponent<ActionMenu>();
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

            var orderList = new List<BaseOrder>
            {
                new RestOrder("Rest", null, direction, worldCoord),
                new WalkToOrder("WalkTo", null, direction, worldCoord), 
            };
            
            var hit = Physics2D.OverlapPoint(mousePos);
            if (hit == null) return;
            var targetCharacter = hit.GetComponent<Character>();
            if (targetCharacter != null)
            {
                orderList.Add(new AttackCharacterOrder("Attack", targetCharacter, direction, worldCoord));
            }
            
            ActionMenu.StartUp(worldCoord, orderList);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            var order = ActionMenu.EndUp();
            PlayerController.CurrentOrder = order;
        }
    }
}