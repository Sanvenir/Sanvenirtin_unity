using ObjectScripts.CharSubstance;
using UtilScripts;

namespace ObjectScripts.ActionScripts
{
    /// <inheritdoc />
    /// <summary>
    ///     An action makes character walk to given TargetDirection
    /// </summary>
    public class WalkAction : BaseAction
    {
        private readonly Direction _targetDirection;

        /// <inheritdoc />
        /// <summary>
        ///     if the Move Check of character on the given TargetDirection is false, return false; else do the action and return
        ///     true
        /// </summary>
        /// <returns></returns>
        public override bool DoAction()
        {
            base.DoAction();
            if (!Self.MoveCheck(Utils.DirectionToVector(_targetDirection))) return false;
            Self.Move(Utils.DirectionToVector(_targetDirection), CostTime);
            return true;
        }

        public WalkAction(Character self, Direction targetDirection) : base(self)
        {
            _targetDirection = targetDirection;
            CostTime = Self.Properties.GetMoveTime();
        }
    }
}