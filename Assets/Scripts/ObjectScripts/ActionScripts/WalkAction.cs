using ObjectScripts.CharSubstance;
using UtilScripts;

namespace ObjectScripts.ActionScripts
{
    public class WalkAction: BaseAction
    {
        private readonly Direction _targetDirection;
        
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