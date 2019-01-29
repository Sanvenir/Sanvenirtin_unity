using ObjectScripts.CharSubstance;
using UtilScripts;
using UtilScripts.Text;

namespace ObjectScripts.StyleScripts.MoveStyleScripts
{
    /// <summary>
    /// Walking is a move style that can recovering endure like wait
    /// </summary>
    public class WalkMoveStyle: BaseMoveStyle
    {
        public override string GetTextName()
        {
            return GameText.Instance.WalkMoveStyle;
        }

        public WalkMoveStyle(Character self) : base(self)
        {
        }

        public override bool MoveAction(Direction targetDirection)
        {
            if (!Self.MoveCheck(Utils.DirectionToVector(targetDirection))) return false;
            Self.Move(Utils.DirectionToVector(targetDirection), MoveTime());
            Self.Recovering(10f);
            return true;
        }

        public override int MoveTime()
        {
            return Self.Properties.GetReactTime(0) * 10;
        }
    }
}