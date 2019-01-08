using ObjectScripts.ActionScripts;
using UtilScripts;

namespace ObjectScripts.CharacterController.CtrlConditions
{
    /// <summary>
    ///     Condition machine for AI
    /// </summary>
    public abstract class BaseCondition
    {
        protected readonly AiController Controller;

        public BaseCondition(AiController controller)
        {
            Controller = controller;
        }

        /// <summary>
        ///     Get next action on the update time of AI
        /// </summary>
        /// <returns>Next action for this AI</returns>
        public abstract BaseAction NextAction();
    }
}