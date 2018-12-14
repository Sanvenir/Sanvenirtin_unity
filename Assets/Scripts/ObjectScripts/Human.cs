using System;
using ObjectScripts.ActionScripts;
using ObjectScripts.BodyPartScripts;
using UnityEngine;

namespace ObjectScripts
{
    public class Human : Character
    {
        // BodyParts

        public override void RefreshProperties()
        {
            base.RefreshProperties();
        }

        public override void Initialize(Vector2Int worldCoord, int areaIdentity)
        {
            base.Initialize(worldCoord, areaIdentity);

            BodyParts.Add("Top", HumanBodyPart.CreateHead());
            BodyParts.Add("TopLow", HumanBodyPart.CreateNeck());
            BodyParts.Add("Center", HumanBodyPart.CreateChest());
            BodyParts.Add("CenterLow", HumanBodyPart.CreateWaist());
            BodyParts.Add("CenterLeft", HumanBodyPart.CreateLeftArm());
            BodyParts.Add("CenterLeftLow", HumanBodyPart.CreateLeftHand());
            BodyParts.Add("CenterRight", HumanBodyPart.CreateRightArm());
            BodyParts.Add("CenterRightLow", HumanBodyPart.CreateRightHand());
            BodyParts.Add("Low", HumanBodyPart.CreateCrotch());
            BodyParts.Add("LowLeft", HumanBodyPart.CreateLeftLeg());
            BodyParts.Add("LowLeftLow", HumanBodyPart.CreateLeftFoot());
            BodyParts.Add("LowRight", HumanBodyPart.CreateRightLeg());
            BodyParts.Add("LowRightLow", HumanBodyPart.CreateRightFoot());

            BodyParts["CenterLeft"].AttachBodyPart = BodyParts["CenterLeftLow"];
            BodyParts["CenterRight"].AttachBodyPart = BodyParts["CenterRightLow"];
            BodyParts["LowLeft"].AttachBodyPart = BodyParts["LowLeftLow"];
            BodyParts["LowRight"].AttachBodyPart = BodyParts["LowRightLow"];
            
        }
    }
}