using ObjectScripts.ActionScripts;
using ObjectScripts.BodyPartScripts;
using ObjectScripts.CharSubstance;
using UnityEngine;
using UtilScripts;

namespace ObjectScripts.CharacterController.CtrlConditions
{
    /// <inheritdoc />
    /// <summary>
    /// AI chase and attack the target character
    /// </summary>
    public class AttackCondition: ChaseCondition
    {
        public AttackCondition(AiController controller, Character target) : base(controller, target)
        {
        }

        public override BaseAction NextAction()
        {
            if (TargetCharacter == null || TargetCharacter.Dead)
            {
                return Controller.SetCondition();
            }
            
            // Control Endure
            var check = Utils.ProcessRandom.Next((int) Self.Properties.GetMaxEndure(0));
            if (Self.Endure > check)
                return new WaitAction(Self);

            if (!Controller.Character.IsVisible(TargetCharacter))
                return base.NextAction();
            var delta = TargetCharacter.WorldCoord - Controller.Character.WorldCoord;
            if(delta.sqrMagnitude == 1)
                return new AttackNeighbourAction(
                    Self, 
                    Utils.VectorToDirection(delta));
            return base.NextAction();
        }
    }
}