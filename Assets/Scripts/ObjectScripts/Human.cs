using System;
using ObjectScripts.ActionScripts;
using ObjectScripts.BodyPartScripts;
using UnityEngine;

namespace ObjectScripts
{
    public class Human : Character
    {
        // BodyParts
//        public BodyPart Head;
//        public BodyPart Neck;
//        public BodyPart Chest;
//        public BodyPart Waist;
//        public BodyPart Crotch;
//        public BodyPart LeftArm;
//        public BodyPart LeftHand;
//        public BodyPart RightArm;
//        public BodyPart RightHand;
//        public BodyPart LeftLeg;
//        public BodyPart LeftFoot;
//        public BodyPart RightLeg;
//        public BodyPart RightFoot;

        public override void RefreshProperties()
        {
            base.RefreshProperties();
        }
        
        public override void Initialize(Vector2Int worldCoord, int areaIdentity)
        {
            base.Initialize(worldCoord, areaIdentity);
            HumanBodyPart.CreateHumanBodyPart(this);
            foreach (var part in GetAllBodyParts())
            {
                BodyParts.Add(part.Name, part);
            }
        }
    }
}