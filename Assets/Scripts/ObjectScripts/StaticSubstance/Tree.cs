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
            base.Initialize(worldCoord, areaIdentity);
            MiddleParts.Add(TreeBody);
            LowParts.Add(TreeStump);

            TreeStump.AttachBodyPart = TreeBody;
            
            BodyParts.Add(TreeBody.Name, TreeBody);
            BodyParts.Add(TreeStump.Name, TreeStump);
        }
    }
}