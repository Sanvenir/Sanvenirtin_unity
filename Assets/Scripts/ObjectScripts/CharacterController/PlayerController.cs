using ObjectScripts.ActionScripts;
using UnityEngine;
using UtilScripts;

namespace ObjectScripts.CharacterController
{
    public class PlayerController: CharacterController
    {
        public BasicAction NextAction;

        private void LateUpdate()
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (Input.GetKey(KeyCode.A))
                {
                    Character.AttackAction.TargetDirection = Direction.Left;
                    NextAction = Character.AttackAction;
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    Character.AttackAction.TargetDirection = Direction.Right;
                    NextAction = Character.AttackAction;
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    Character.AttackAction.TargetDirection = Direction.Down;
                    NextAction = Character.AttackAction;
                }
                else if (Input.GetKey(KeyCode.W))
                {
                    Character.AttackAction.TargetDirection = Direction.Up;
                    NextAction = Character.AttackAction;
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
                    Character.WalkAction.TargetDirection = Direction.Left;
                    NextAction = Character.WalkAction;
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    Character.WalkAction.TargetDirection = Direction.Right;
                    NextAction = Character.WalkAction;
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    Character.WalkAction.TargetDirection = Direction.Down;
                    NextAction = Character.WalkAction;
                }
                else if (Input.GetKey(KeyCode.W))
                {
                    Character.WalkAction.TargetDirection = Direction.Up;
                    NextAction = Character.WalkAction;
                }
                else if (Input.GetKey(KeyCode.Space))
                {
                    NextAction = Character.WaitAction;
                }
                else
                {
                    NextAction = null;
                }
            }
        }

        public override void UpdateFunction()
        {
            if (NextAction == null) return;
            NextAction.DoAction();
            NextAction = null;
        }
    }
}