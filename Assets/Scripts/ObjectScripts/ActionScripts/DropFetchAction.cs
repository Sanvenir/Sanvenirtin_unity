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
        private readonly BaseObject _targetObject;

        public DropFetchAction(Character self, BaseObject targetObject, BodyPart fetchPart) : base(self)
        {
            _targetObject = targetObject;
            _fetchPart = fetchPart;
            CostTime = Self.Properties.GetActTime(0.1f);
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <returns>If given FetchPart is not in the fetch dictionary of the character, return false</returns>
        public override bool DoAction()
        {
            base.DoAction();
            if (!Self.FetchDictionary.ContainsKey(_fetchPart)) return false;
            Self.FetchDictionary[_fetchPart] = null;
            _targetObject.transform.position = Self.WorldPos;
            _targetObject.gameObject.SetActive(true);
            return true;
        }
    }
}