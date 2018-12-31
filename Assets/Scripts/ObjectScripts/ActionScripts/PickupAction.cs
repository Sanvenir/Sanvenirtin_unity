using ObjectScripts.BodyPartScripts;
using ObjectScripts.CharSubstance;

namespace ObjectScripts.ActionScripts
{
    public class PickupAction: BaseAction
    {
        private BodyPart _fetchPart;
        public PickupAction(Character self) : base(self)
        {
        }

        public override void DoAction(bool check = true)
        {
            CostTime = Self.GetActTime();
            Self.ActivateTime += CostTime;
            if (!check)
            {
                Self.FetchDictionary[_fetchPart] = TargetObject;
                TargetObject.gameObject.SetActive(false);
                return;
            }
            
            foreach (var key in Self.FetchDictionary.Keys)
            {
                if (!key.Available) continue;
                if (Self.FetchDictionary[key] != null) continue;
                Self.FetchDictionary[key] = TargetObject;
                TargetObject.gameObject.SetActive(false);
                
                return;
            }
        }

        public override bool CheckAction()
        {
            foreach (var key in Self.FetchDictionary.Keys)
            {
                if (!key.Available) continue;
                if (Self.FetchDictionary[key] != null) continue;
                _fetchPart = key;
                return true;
            }   
            SceneManager.Instance.Print("You have no hand to fetch this item");
            return false;
        }
    }
}