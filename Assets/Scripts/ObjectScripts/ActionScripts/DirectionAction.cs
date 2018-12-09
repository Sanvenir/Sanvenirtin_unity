using UtilScripts;

namespace ObjectScripts.ActionScripts
{
    public abstract class DirectionAction: BasicAction
    {
        public Direction TargetDirection;

        protected DirectionAction(Character self) : base(self)
        {
        }
    }
}