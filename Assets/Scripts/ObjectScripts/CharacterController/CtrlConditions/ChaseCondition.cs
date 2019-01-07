using ObjectScripts.ActionScripts;
using ObjectScripts.CharSubstance;
using UnityEngine;
using UtilScripts;

namespace ObjectScripts.CharacterController.CtrlConditions
{
    /// <inheritdoc />
    /// <summary>
    /// AI Chase the Target Character
    /// </summary>
    public class ChaseCondition : BaseCondition
    {
        protected readonly Character TargetCharacter;

        public ChaseCondition(AiController controller, Character target) : base(controller)
        {
            TargetCharacter = target;
        }

        public override BaseAction NextAction()
        {
            if (TargetCharacter == null || TargetCharacter.Dead)
            {
                Controller.Condition = new WonderCondition(Controller);
                return Controller.Condition.NextAction();
            }

            var incVec = Controller.AStarFinder(
                TargetCharacter.WorldCoord,
                (int) Controller.Character.Properties.Intelligence);

            return incVec == Vector2Int.zero
                ? base.NextAction()
                : new WalkAction(Controller.Character, Utils.VectorToDirection(incVec));
        }
    }
}