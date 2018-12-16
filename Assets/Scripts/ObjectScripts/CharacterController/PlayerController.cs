using ObjectScripts.ActionScripts;
using ObjectScripts.CharacterController.PlayerOrder;
using UnityEngine;
using UtilScripts;

namespace ObjectScripts.CharacterController
{
    public class PlayerController: CharacterController
    {
        public BaseAction NextAction;
        public BaseOrder CurrentOrder;
        
        [HideInInspector]
        public Vector2Int TargetCoord;
        [HideInInspector]
        public Character TargetCharacter;
        [HideInInspector]
        public Direction TargetDirection;

        private Vector2Int _vecInt;

        protected override void Awake()
        {
            base.Awake();
            BaseOrder.Controller = this;
        }
        
        private void LateUpdate()
        {
            
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (Input.GetKey(KeyCode.A))
                {
                    AttackAction.TargetDirection = Direction.Left;
                    NextAction = AttackAction;
                    CurrentOrder = null;
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    AttackAction.TargetDirection = Direction.Right;
                    NextAction = AttackAction;
                    CurrentOrder = null;
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    AttackAction.TargetDirection = Direction.Down;
                    NextAction = AttackAction;
                    CurrentOrder = null;
                }
                else if (Input.GetKey(KeyCode.W))
                {
                    AttackAction.TargetDirection = Direction.Up;
                    NextAction = AttackAction;
                    CurrentOrder = null;
                }
                else
                {
                    NextAction = null;
                }
            }
            else
            {
                if (Input.GetKey(KeyCode.A))
                {
                    WalkAction.TargetDirection = Direction.Left;
                    NextAction = WalkAction;
                    CurrentOrder = null;
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    WalkAction.TargetDirection = Direction.Right;
                    NextAction = WalkAction;
                    CurrentOrder = null;
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    WalkAction.TargetDirection = Direction.Down;
                    NextAction = WalkAction;
                    CurrentOrder = null;
                }
                else if (Input.GetKey(KeyCode.W))
                {
                    WalkAction.TargetDirection = Direction.Up;
                    NextAction = WalkAction;
                    CurrentOrder = null;
                }
                else if (Input.GetKey(KeyCode.Space))
                {
                    NextAction = WaitAction;
                    CurrentOrder = null;
                }
                else
                {
                    NextAction = null;
                }
            }

            if (CurrentOrder == null) return;
            CurrentOrder = CurrentOrder.DoOrder();
        }

        public override void UpdateFunction()
        {
            if (NextAction == null) return;
            NextAction.DoAction();
            NextAction = null;
        }
    }
}