using ObjectScripts.BodyPartScripts;
using UnityEngine;

namespace ObjectScripts.StaticSubstance
{
    public class Tree: Substance
    {
        public BodyPart TreeBody;
        public BodyPart TreeStump;
        
        public override void Initialize(Vector2Int worldCoord, int areaIdentity)
        {
            BodyParts.Add(TreeBody.Name, (BodyPart) TreeBody.Create(this));
            BodyParts.Add(TreeStump.Name, (BodyPart) TreeStump.Create(this));
            
            base.Initialize(worldCoord, areaIdentity);
        }
    }
}