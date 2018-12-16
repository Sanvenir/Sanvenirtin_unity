using ObjectScripts.ActionScripts;
using UtilScripts;

namespace ObjectScripts.CharacterController.CtrlConditions
{
    public class AttackCondition: ChaseCondition
    {

        public AttackCondition(AiController controller, Character target) : base(controller, target)
        {
        }

        public override BaseAction NextAction()
        {
            if (TargetCharacter == null || TargetCharacter.Dead)
            {
                Controller.Condition = new BaseCondition(Controller);
                return Controller.Condition.NextAction();
            }

            Controller.AttackAction.TargetDirection =
                Utils.VectorToDirection(TargetCharacter.WorldCoord - Controller.Character.WorldCoord);
            if (Controller.AttackAction.CheckAction() && 
                Controller.AttackAction.Target == TargetCharacter)
            {
                return Controller.AttackAction;
            }
            return base.NextAction();
        }
    }
}