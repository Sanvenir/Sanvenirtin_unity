using ObjectScripts.ActionScripts;
using UnityEngine;
using UtilScripts;

namespace ObjectScripts.CharacterController.CtrlConditions
{
    public class ChaseCondition: BaseCondition
    {
        public Character TargetCharacter;
        
        public ChaseCondition(AiController controller, Character target) : base(controller)
        {
            TargetCharacter = target;
        }

        public override BaseAction NextAction()
        {
            if (TargetCharacter == null || TargetCharacter.Dead)
            {
                Controller.Condition = new BaseCondition(Controller);
                return Controller.Condition.NextAction();
            }
            var incVec = Controller.AStarFinder(
                TargetCharacter.WorldCoord, (int)Controller.Character.Intelligence);

            if (incVec == Vector2Int.zero) return base.NextAction();
            
            Controller.WalkAction.TargetDirection = Utils.VectorToDirection(incVec);
            return Controller.WalkAction;

        }
    }
}