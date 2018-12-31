using ObjectScripts.ActionScripts;
using ObjectScripts.BodyPartScripts;
using ObjectScripts.CharSubstance;
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
            Controller.AttackAction.AttackPart = (PartPos) Utils.ProcessRandom.Next(3);
            if (Controller.AttackAction.CheckAction() && 
                Controller.AttackAction.Target == TargetCharacter)
            {
                return Controller.AttackAction;
            }
            return base.NextAction();
        }
    }
}