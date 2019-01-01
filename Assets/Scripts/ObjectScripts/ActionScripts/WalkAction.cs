using ObjectScripts.CharSubstance;
using UtilScripts;

namespace ObjectScripts.ActionScripts
{
    public class WalkAction: BaseAction
    {
        public override void DoAction(bool check = true)
        {
            CostTime = Self.Properties.GetMoveTime();
            Self.ActivateTime += CostTime;
            Self.Move(Utils.DirectionToVector(TargetDirection), CostTime, check);
        }

        public override bool CheckAction()
        {
            // If can move, return true
            return Self.MoveCheck(Utils.DirectionToVector(TargetDirection));
        }

        public WalkAction(Character self) : base(self)
        {
        }
    }
}