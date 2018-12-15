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
            HumanBodyPart.CreateHumanBodyPart(this);
            foreach (var part in GetAllBodyParts())
            {
                BodyParts.Add(part.Name, part);
            }
        }
    }
}