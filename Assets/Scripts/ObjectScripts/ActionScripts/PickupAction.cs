using ObjectScripts.BodyPartScripts;
using ObjectScripts.CharSubstance;

namespace ObjectScripts.ActionScripts
{
    /// <inheritdoc />
    /// <summary>
    ///     An action makes character pick an object up
    /// </summary>
    public class PickupAction : BaseAction
    {
        private readonly BodyPart _fetchPart;
        private readonly BaseObject _targetObject;

        public PickupAction(Character self, BaseObject targetObject, BodyPart fetchPart) : base(self)
        {
            _targetObject = targetObject;
            _fetchPart = fetchPart;
            CostTime = Self.Properties.GetActTime(0.1f);
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <returns>
        ///     If fetch dictionary of character do not have the key of given fetch part, or the value of relative fetch part
        ///     is not null return false, else do action and return true
        /// </returns>
        public override bool DoAction()
        {
            base.DoAction();
            if (!_fetchPart.Available || !Self.FetchDictionary.ContainsKey(_fetchPart) || Self.FetchDictionary[_fetchPart] != null) return false;
            Self.FetchDictionary[_fetchPart] = _targetObject;
            _targetObject.gameObject.SetActive(false);
            return true;
        }
    }
}