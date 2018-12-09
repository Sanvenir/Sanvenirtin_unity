using UtilScripts;

namespace ObjectScripts.ActionScripts
{
    public class WalkAction: DirectionAction
    {
        public override void DoAction(bool check = true)
        {
            CostTime = Self.GetMoveTime();
            Self.ActivateTime += CostTime;
            Self.Move(Utils.DirectionToVector(TargetDirection), CostTime, check);
        }

        public override bool CheckAction()
        {
            return Self.MoveCheck<Substance>(Utils.DirectionToVector(TargetDirection)) == null;
        }

        public WalkAction(Character self) : base(self)
        {
        }
    }
}