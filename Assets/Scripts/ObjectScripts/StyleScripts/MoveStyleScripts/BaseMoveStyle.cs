using ObjectScripts.CharSubstance;
using UtilScripts;

namespace ObjectScripts.StyleScripts.MoveStyleScripts
{
    public abstract class BaseMoveStyle : BaseStyle
    {
        protected BaseMoveStyle(Character self) : base(self)
        {
        }

        public abstract bool MoveAction(Direction targetDirection);
        public abstract int MoveTime();
    }
}