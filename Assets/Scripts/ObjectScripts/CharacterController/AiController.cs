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
                WaitAction.DoAction();
                return;
            }

            WalkAction.TargetDirection = (Direction) Utils.ProcessRandom.Next(4);
            if (WalkAction.CheckAction())
            {
                WalkAction.DoAction();
                return;
            }

            WaitAction.DoAction();

        }
    }
}