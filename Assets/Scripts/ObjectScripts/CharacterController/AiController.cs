using ObjectScripts.ActionScripts;
using ObjectScripts.CharacterController.CtrlConditions;
using ObjectScripts.CharSubstance;
using UnityEngine;
using UtilScripts;

namespace ObjectScripts.CharacterController
{
    public class AiController: CharacterController
    {
        public BaseCondition DefaultCondition;
        public BaseCondition Condition;

        private void Start()
        {
            DefaultCondition = new AlertOtherRaceCondition(this);
            Condition = DefaultCondition;
        }

        /// <summary>
        /// Set condition and return its next action
        /// </summary>
        /// <param name="condition">New condition, default DefaultCondition</param>
        /// <returns>The next action of new condition</returns>
        public BaseAction SetCondition(BaseCondition condition = null)
        {
            Condition = condition ?? DefaultCondition;
            return Condition.NextAction();
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