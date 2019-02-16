using ObjectScripts.CharSubstance;
using UtilScripts;
using UtilScripts.Text;

namespace ObjectScripts.StyleScripts.MoveStyleScripts
{
    /// <summary>
    ///     Dashing is a movement makes possible fastest movement but cost large amount of endure
    /// </summary>
    public class DashMoveStyle : BaseMoveStyle
    {
        public override string GetTextName()
        {
            return GameText.Instance.DashMoveStyle;
        }

        public DashMoveStyle(Character self) : base(self)
        {
        }

        public override bool MoveAction(Direction targetDirection)
        {
            if (!Self.MoveCheck(Utils.DirectionToVector(targetDirection))) return false;
            Self.Move(Utils.DirectionToVector(targetDirection), MoveTime());
            Self.Endure += Self.Properties.MoveSpeed.Use(1);
            return true;
        }

        public override int MoveTime()
        {
            return Self.Properties.GetMoveTime(0);
        }
    }
}