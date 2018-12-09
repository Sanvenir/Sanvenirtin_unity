using System;
using ObjectScripts.ActionScripts;
using ObjectScripts.ComponentScripts;
using UnityEngine;

namespace ObjectScripts
{
    public class Human : Character
    {
        // Components

        public override void RefreshProperties()
        {
            base.RefreshProperties();
        }

        public override void Initialize(Vector2Int globalCoord, int areaIdentity)
        {
            base.Initialize(globalCoord, areaIdentity);

            Components.Add("Top", HumanComponents.createHead());
            Components.Add("TopLow", HumanComponents.createNeck());
            Components.Add("Center", HumanComponents.createChest());
            Components.Add("CenterLow", HumanComponents.createWaist());
            Components.Add("CenterLeft", HumanComponents.createLeftArm());
            Components.Add("CenterLeftLow", HumanComponents.createLeftHand());
            Components.Add("CenterRight", HumanComponents.createRightArm());
            Components.Add("CenterRightLow", HumanComponents.createRightHand());
            Components.Add("Low", HumanComponents.createCrotch());
            Components.Add("LowLeft", HumanComponents.createLeftLeg());
            Components.Add("LowLeftLow", HumanComponents.createLeftFoot());
            Components.Add("LowRight", HumanComponents.createRightLeg());
            Components.Add("LowRightLow", HumanComponents.createRightFoot());

            Components["CenterLeft"].AttachSubstanceComponent = Components["CenterLeftLow"];
            Components["CenterRight"].AttachSubstanceComponent = Components["CenterRightLow"];
            Components["LowLeft"].AttachSubstanceComponent = Components["LowLeftLow"];
            Components["LowRight"].AttachSubstanceComponent = Components["LowRightLow"];
            
        }
    }
}