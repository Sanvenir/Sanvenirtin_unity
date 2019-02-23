using ObjectScripts.ActionScripts;
using UtilScripts;

namespace ObjectScripts.CharacterController.CtrlConditions
{
    /// <inheritdoc />
    /// <summary>
    ///     AI walks around randomly
    /// </summary>
    public class WonderCondition : BaseCondition
    {
        public WonderCondition(AiController controller) : base(controller)
        {
        }

        public override BaseAction NextAction()
        {
            if (Utils.ProcessRandom.Next(5) != 0) return new WaitAction(Self);

            return new MoveAction(Self, (Direction) Utils.ProcessRandom.Next(4));
        }
    }
}