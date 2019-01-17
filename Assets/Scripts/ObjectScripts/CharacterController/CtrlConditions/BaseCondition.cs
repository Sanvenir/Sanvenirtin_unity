using ObjectScripts.ActionScripts;
using ObjectScripts.CharSubstance;
using UtilScripts;

namespace ObjectScripts.CharacterController.CtrlConditions
{
    /// <summary>
    ///     Condition machine for AI
    /// </summary>
    public abstract class BaseCondition
    {
        protected readonly AiController Controller;

        protected Character Self
        {
            get { return Controller.Character; }
        }

        public BaseCondition(AiController controller)
        {
            Controller = controller;
        }

        /// <summary>
        ///     Use next action on the update time of AI
        /// </summary>
        /// <returns>Next action for this AI</returns>
        public abstract BaseAction NextAction();
    }
}