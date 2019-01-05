using ObjectScripts.ActionScripts;
using UtilScripts;

namespace ObjectScripts.CharacterController.CtrlConditions
{
    public class WonderCondition: BaseCondition
    {
        public WonderCondition(AiController controller) : base(controller)
        {
        }

        public override BaseAction NextAction()
        {
            if (Utils.ProcessRandom.Next(5) != 0)
            {
                return new WaitAction(Controller.Character);
            }

            return new WalkAction(Controller.Character, (Direction) Utils.ProcessRandom.Next(4));
        }
    }
}