using ObjectScripts.CharacterController.CtrlConditions;
using ObjectScripts.CharSubstance;
using UnityEngine;
using UtilScripts;

namespace ObjectScripts.CharacterController
{
    public class AiController: CharacterController
    {
        public BaseCondition Condition;

        private void Start()
        {
            Condition = new WonderCondition(this);
        }

        public override void UpdateFunction()
        {
            Condition.NextAction().DoAction();
        }

        public override void GetHostility(Character hostility, int level)
        {
            base.GetHostility(hostility, level);
            Condition = new AttackCondition(this, hostility);
        }
    }
}