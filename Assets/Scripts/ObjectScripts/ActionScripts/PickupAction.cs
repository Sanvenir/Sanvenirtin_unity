
using System.Collections.Generic;
using System.Linq;
using ObjectScripts.BodyPartScripts;
using ObjectScripts.CharSubstance;

namespace ObjectScripts.ActionScripts
{
    public class PickupAction: BaseAction
    {
        private readonly BodyPart _fetchPart;
        private readonly BaseObject _targetObject;
        
        public PickupAction(Character self, BaseObject targetObject, BodyPart fetchPart) : base(self)
        {
            _targetObject = targetObject;
            _fetchPart = fetchPart;
            CostTime = Self.Properties.GetActTime();
        }

        public override bool DoAction()
        {
            base.DoAction();
            // If check is true, this action check whether any BodyParts is available (for player), 
            //    so the RefreshFetchParts should be called first, and body part index should
            //    be chosen;
            if (!Self.FetchDictionary.ContainsKey(_fetchPart)) return false;

            Self.FetchDictionary[_fetchPart] = _targetObject;
            _targetObject.gameObject.SetActive(false);
            return true;
        }
    }
}