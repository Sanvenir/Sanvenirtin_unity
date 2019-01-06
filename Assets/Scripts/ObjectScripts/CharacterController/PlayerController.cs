using ObjectScripts.ActionScripts;
using ObjectScripts.CharacterController.PlayerOrder;
using ObjectScripts.CharSubstance;
using UnityEngine;
using UtilScripts;

namespace ObjectScripts.CharacterController
{
    public class PlayerController: CharacterController
    {
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
        }
        
        private void LateUpdate()
        {
//            
//            if (Input.GetKey(KeyCode.LeftShift))
//            {
//                if (Input.GetKey(KeyCode.A))
//                {
//                    AttackNeighbourAction.TargetDirection = Direction.Left;
//                    NextAction = AttackNeighbourAction;
//                    CurrentOrder = null;
//                }
//                else if (Input.GetKey(KeyCode.D))
//                {
//                    AttackNeighbourAction.TargetDirection = Direction.Right;
//                    NextAction = AttackNeighbourAction;
//                    CurrentOrder = null;
//                }
//                else if (Input.GetKey(KeyCode.S))
//                {
//                    AttackNeighbourAction.TargetDirection = Direction.Down;
//                    NextAction = AttackNeighbourAction;
//                    CurrentOrder = null;
//                }
//                else if (Input.GetKey(KeyCode.W))
//                {
//                    AttackNeighbourAction.TargetDirection = Direction.Up;
//                    NextAction = AttackNeighbourAction;
//                    CurrentOrder = null;
//                }
//                else
//                {
//                    NextAction = null;
//                }
//            }
//            else
//            {
//                if (Input.GetKey(KeyCode.A))
//                {
//                    WalkAction.TargetDirection = Direction.Left;
//                    NextAction = WalkAction;
//                    CurrentOrder = null;
//                }
//                else if (Input.GetKey(KeyCode.D))
//                {
//                    WalkAction.TargetDirection = Direction.Right;
//                    NextAction = WalkAction;
//                    CurrentOrder = null;
//                }
//                else if (Input.GetKey(KeyCode.S))
//                {
//                    WalkAction.TargetDirection = Direction.Down;
//                    NextAction = WalkAction;
//                    CurrentOrder = null;
//                }
//                else if (Input.GetKey(KeyCode.W))
//                {
//                    WalkAction.TargetDirection = Direction.Up;
//                    NextAction = WalkAction;
//                    CurrentOrder = null;
//                }
//                else if (Input.GetKey(KeyCode.Space))
//                {
//                    NextAction = WaitAction;
//                    CurrentOrder = null;
//                }
//                else
//                {
//                    NextAction = null;
//                }
//            }

        }

        public override void SetAction(BaseAction action)
        {
            base.SetAction(action);
            CurrentOrder = null;
        }

        public override void UpdateFunction()
        {

            if (CurrentOrder != null)
            {
                CurrentOrder = CurrentOrder.DoOrder();
            }

            if (NextAction == null) return;
            NextAction.DoAction();
            NextAction = null;
        }
    }
}