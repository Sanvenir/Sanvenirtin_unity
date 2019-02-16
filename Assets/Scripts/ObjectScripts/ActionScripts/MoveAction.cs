using ObjectScripts.CharSubstance;
using UtilScripts;

namespace ObjectScripts.ActionScripts
{
    /// <inheritdoc />
    /// <summary>
    ///     An action makes character walk to given TargetDirection
    /// </summary>
    public class MoveAction : BaseAction
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
            Self.ActivateTime += CostTime;
            return Self.CurrentMoveStyle.MoveAction(_targetDirection);
        }

        public MoveAction(Character self, Direction targetDirection) : base(self)
        {
            _targetDirection = targetDirection;
            CostTime = Self.CurrentMoveStyle.MoveTime();
        }
    }
}