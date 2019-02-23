using ObjectScripts.ActionScripts;
using ObjectScripts.CharSubstance;
using UnityEngine;
using UtilScripts;

namespace ObjectScripts.CharacterController.CtrlConditions
{
    /// <inheritdoc />
    /// <summary>
    ///     AI Chase the Target Parent
    /// </summary>
    public class ChaseCondition : WonderCondition
    {
        protected readonly Character TargetCharacter;

        public ChaseCondition(AiController controller, Character target) : base(controller)
        {
            TargetCharacter = target;
        }

        public override BaseAction NextAction()
        {
            if (TargetCharacter == null || TargetCharacter.Dead) return Controller.SetCondition();

            if (!Controller.Character.IsVisible(TargetCharacter)) return base.NextAction();

            var incVec = Controller.AStarFinder(
                TargetCharacter.WorldCoord,
                (int) Self.Properties.Intelligence.Use(0.1f));

            return incVec == Vector2Int.zero
                ? base.NextAction()
                : new MoveAction(Self, Utils.VectorToDirection(incVec));
        }
    }
}