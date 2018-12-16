using ObjectScripts.ActionScripts;
using UtilScripts;

namespace ObjectScripts.CharacterController.CtrlConditions
{
    public class BaseCondition
    {
        public AiController Controller;
        
        public BaseCondition(AiController controller)
        {
            Controller = controller;
        }

        public virtual BaseAction NextAction()
        {
            if (Utils.ProcessRandom.Next(5) != 0)
            {
                return Controller.WaitAction;
            }

            
            Controller.WalkAction.TargetDirection = (Direction) Utils.ProcessRandom.Next(4);
            return Controller.WalkAction;
        }
        
        
    }
}