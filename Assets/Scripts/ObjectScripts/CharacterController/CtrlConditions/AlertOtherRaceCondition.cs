using ObjectScripts.ActionScripts;
using ObjectScripts.CharSubstance;

namespace ObjectScripts.CharacterController.CtrlConditions
{
    /// <inheritdoc />
    /// <summary>
    ///     This condition is for character that ai will attack other character if they are different in race, else act as
    ///     wonder condition
    /// </summary>
    public class AlertOtherRaceCondition : WonderCondition
    {
        public AlertOtherRaceCondition(AiController controller) : base(controller)
        {
        }

        public override BaseAction NextAction()
        {
            foreach (var character in Self.GetVisibleObjects<Character>())
            {
                if (character.RaceIndex == Self.RaceIndex || character.Dead) continue;
                return Controller.SetCondition(new AttackCondition(Controller, character));
            }

            return base.NextAction();
        }
    }
}