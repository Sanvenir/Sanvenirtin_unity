using ObjectScripts.CharacterController.CtrlConditions;
using UnityEngine;
using UtilScripts;

namespace ObjectScripts.CharacterController
{
    public class AiController: CharacterController
    {

        public override void UpdateFunction()
        {
            if (Utils.ProcessRandom.Next(5) == 0)
            {
                Character.WaitAction.DoAction();
                return;
            }

            Character.WalkAction.TargetDirection = (Direction) Utils.ProcessRandom.Next(4);
            if (Character.WalkAction.CheckAction())
            {
                Character.WalkAction.DoAction();
                return;
            }

            Character.WaitAction.DoAction();

        }
    }
}