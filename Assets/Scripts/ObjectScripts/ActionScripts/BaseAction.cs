using ObjectScripts.CharSubstance;

namespace ObjectScripts.ActionScripts
{
    /// <summary>
    ///     Actions for characters. Every movement of a character is an action, when the global time greater than or equal to
    ///     the activate time of character, i.e. the character's turn has come, the character can choose and do the action
    /// </summary>
    public abstract class BaseAction
    {
        protected readonly Character Self;

        protected int CostTime;

        public BaseAction(Character self)
        {
            Self = self;
        }

        /// <summary>
        ///     Parent's activate time need to be increased if the action has been done(or not)
        /// </summary>
        /// <returns>Whether the action is successful</returns>
        public abstract bool DoAction();
    }
}