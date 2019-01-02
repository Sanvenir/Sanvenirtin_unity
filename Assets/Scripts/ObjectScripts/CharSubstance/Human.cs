using ObjectScripts.BodyPartScripts;
using UnityEngine;

namespace ObjectScripts.CharSubstance
{
    public class Human : Character
    {
        public override void RefreshProperties()
        {
            base.RefreshProperties();
        }
        
        public override void Initialize(Vector2Int worldCoord, int areaIdentity)
        {
//
//            BodyParts.Add("Head", Head);
//            BodyParts.Add("LeftArm", LeftArm);
//            BodyParts.Add("RightArm", RightArm);
//            BodyParts.Add("LeftHand", LeftHand);
//            BodyParts.Add("RightHand", RightHand);
//
//            LeftArm.AttachBodyPart = LeftHand.Name;
//            RightArm.AttachBodyPart = RightHand.Name;
//
//            FetchDictionary.Add(LeftHand, null);
//            FetchDictionary.Add(RightHand, null);
            
            base.Initialize(worldCoord, areaIdentity);
        }
    }
}