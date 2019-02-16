using ObjectScripts.BodyPartScripts;
using ObjectScripts.CharSubstance;

namespace ObjectScripts.ActionScripts
{
    /// <inheritdoc />
    /// <summary>
    ///     An action makes character drop an item on its fetch part
    /// </summary>
    public class DropFetchAction : BaseAction
    {
        private readonly BodyPart _fetchPart;

        public DropFetchAction(Character self, BodyPart fetchPart) : base(self)
        {
            _fetchPart = fetchPart;
            CostTime = Self.Properties.GetActTime(0.1f);
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <returns>If given FetchPart is not in the fetch dictionary of the character, return false</returns>
        public override bool DoAction()
        {
            if (!Self.FetchDictionary.ContainsKey(_fetchPart)) return false;
            Self.DropFetchObject(_fetchPart);
            return true;
        }
    }
}