using ObjectScripts.ActionScripts;
using UtilScripts;

namespace ObjectScripts.CharacterController.CtrlConditions
{
    public abstract class BaseCondition
    {
        protected readonly AiController Controller;
        
        public BaseCondition(AiController controller)
        {
            Controller = controller;
        }

        public virtual BaseAction NextAction()
        {
            return new WalkAction(Controller.Character, (Direction) Utils.ProcessRandom.Next(4));
        }


    }
}