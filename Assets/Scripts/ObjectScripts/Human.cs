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

            Components.Add("Head", new SubstanceComponent());
            Components.Add("Neck", new SubstanceComponent());
            Components.Add("Shoulder", new SubstanceComponent());
            Components.Add("Chest", new SubstanceComponent());
            Components.Add("Waist", new SubstanceComponent());
            Components.Add("LeftArm", new SubstanceComponent());
            Components.Add("LeftHand", new SubstanceComponent());
            Components.Add("RightArm", new SubstanceComponent());
            Components.Add("RightHand", new SubstanceComponent());
            Components.Add("Crotch", new SubstanceComponent());
            Components.Add("LeftLeg", new SubstanceComponent());
            Components.Add("LeftFoot", new SubstanceComponent());
            Components.Add("RightLeg", new SubstanceComponent());
            Components.Add("RightFoot", new SubstanceComponent());

            Components["LeftArm"].AttachSubstanceComponent = Components["LeftHand"];
            Components["RightArm"].AttachSubstanceComponent = Components["RightHand"];
            Components["LeftLeg"].AttachSubstanceComponent = Components["LeftFoot"];
            Components["RightLeg"].AttachSubstanceComponent = Components["RightFoot"];
            
            Components["Head"].Essential = true;
            Components["Neck"].Essential = true;
            Components["Chest"].Essential = true;
            Components["Waist"].Essential = true;
            Components["Crotch"].Essential = true;
            
        }
    }
}