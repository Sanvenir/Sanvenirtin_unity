using ObjectScripts.BodyPartScripts;
using ObjectScripts.CharSubstance;

namespace ObjectScripts.ActionScripts
{
    public class DropAction: BaseAction 
    {
        private readonly BodyPart _fetchPart;
        private readonly BaseObject _targetObject;

        public DropAction(Character self, BaseObject targetObject, BodyPart fetchPart) : base(self)
        {
            _targetObject = targetObject;
            _fetchPart = fetchPart;
            CostTime = Self.Properties.GetActTime();
        }

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