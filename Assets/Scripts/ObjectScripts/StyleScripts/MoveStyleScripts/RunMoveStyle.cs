using ObjectScripts.CharSubstance;
using UtilScripts;
using UtilScripts.Text;

namespace ObjectScripts.StyleScripts.MoveStyleScripts
{
    /// <summary>
    ///     Running is a normal move style
    /// </summary>
    public class RunMoveStyle : BaseMoveStyle
    {
        public RunMoveStyle(Character self) : base(self)
        {
        }

        public override string GetTextName()
        {
            return GameText.Instance.RunMoveStyle;
        }

        public override bool MoveAction(Direction targetDirection)
        {
            if (!Self.MoveCheck(Utils.DirectionToVector(targetDirection))) return false;
            Self.Move(Utils.DirectionToVector(targetDirection), MoveTime());
            return true;
        }

        public override int MoveTime()
        {
            return Self.Properties.GetMoveTime(0) * 2;
        }
    }
}