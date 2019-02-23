using ObjectScripts.ActionScripts;
using ObjectScripts.CharSubstance;

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

        protected Character Self
        {
            get { return Controller.Character; }
        }

        /// <summary>
        ///     Use next action on the update time of AI
        /// </summary>
        /// <returns>Next action for this AI</returns>
        public abstract BaseAction NextAction();
    }
}