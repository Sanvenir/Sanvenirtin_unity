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

        public override void Initialize(Vector2Int worldCoord, int areaIdentity)
        {
            base.Initialize(worldCoord, areaIdentity);

            Components.Add("Top", HumanComponents.CreateHead());
            Components.Add("TopLow", HumanComponents.CreateNeck());
            Components.Add("Center", HumanComponents.CreateChest());
            Components.Add("CenterLow", HumanComponents.CreateWaist());
            Components.Add("CenterLeft", HumanComponents.CreateLeftArm());
            Components.Add("CenterLeftLow", HumanComponents.CreateLeftHand());
            Components.Add("CenterRight", HumanComponents.CreateRightArm());
            Components.Add("CenterRightLow", HumanComponents.CreateRightHand());
            Components.Add("Low", HumanComponents.CreateCrotch());
            Components.Add("LowLeft", HumanComponents.CreateLeftLeg());
            Components.Add("LowLeftLow", HumanComponents.CreateLeftFoot());
            Components.Add("LowRight", HumanComponents.CreateRightLeg());
            Components.Add("LowRightLow", HumanComponents.CreateRightFoot());

            Components["CenterLeft"].AttachSubstanceComponent = Components["CenterLeftLow"];
            Components["CenterRight"].AttachSubstanceComponent = Components["CenterRightLow"];
            Components["LowLeft"].AttachSubstanceComponent = Components["LowLeftLow"];
            Components["LowRight"].AttachSubstanceComponent = Components["LowRightLow"];
            
        }
    }
}