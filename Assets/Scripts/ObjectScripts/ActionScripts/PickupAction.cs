
using System.Collections.Generic;
using ObjectScripts.BodyPartScripts;
using ObjectScripts.CharSubstance;

namespace ObjectScripts.ActionScripts
{
    public class PickupAction: BaseAction
    {
        public BodyPart FetchPart;
        public List<BodyPart> FetchParts;
        public PickupAction(Character self) : base(self)
        {
        }

        public override void DoAction(bool check = true)
        {
            // If check is true, this action check whether any BodyParts is available (for player), 
            //    so the RefreshFetchParts should be called first, and body part index should
            //    be chosen;
            if (FetchParts == null || !FetchParts.Contains(FetchPart))
            {
                if(check)
                    SceneManager.Instance.Print("You cannot fetch this item");
                return;
            }
            
            CostTime = Self.Properties.GetActTime();
            Self.ActivateTime += CostTime;

            Self.FetchDictionary[FetchPart] = TargetObject;
            TargetObject.gameObject.SetActive(false);
        }

        public void RefreshFetchParts()
        {
            FetchParts = new List<BodyPart>();
            foreach (var key in Self.FetchDictionary.Keys)
            {
                if (!key.Available) continue;
                if (Self.FetchDictionary[key] != null) continue;
                FetchParts.Add(key);
            }
        }

        public override bool CheckAction()
        {
            RefreshFetchParts();
            FetchPart = FetchParts[0];
            return FetchParts.Count != 0;
        }
    }
}