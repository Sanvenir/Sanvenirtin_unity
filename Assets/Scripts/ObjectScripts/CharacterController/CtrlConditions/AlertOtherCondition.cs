using ObjectScripts.ActionScripts;
using ObjectScripts.CharSubstance;

namespace ObjectScripts.CharacterController.CtrlConditions
{
    /// <inheritdoc />
    /// <summary>
    /// This condition is for character that ai will attack other character if find any, else act as wonder condition
    /// </summary>
    public class AlertOtherCondition: WonderCondition
    {
        public AlertOtherCondition(AiController controller) : base(controller)
        {
            
        }

        public override BaseAction NextAction()
        {
            foreach (var character in Controller.Character.GetVisibleObjects<Character>())
            {
                if (character == Controller.Character || character.Dead) continue;
                return Controller.SetCondition(new AttackCondition(Controller, character));
            }
            return base.NextAction();
        }
    }
}