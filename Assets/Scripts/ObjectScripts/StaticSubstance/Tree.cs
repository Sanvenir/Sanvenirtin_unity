using ObjectScripts.BodyPartScripts;
using UnityEngine;

namespace ObjectScripts.StaticSubstance
{
    public class Tree: ComplexObject
    {
        public BodyPart TreeBody;
        public BodyPart TreeStump;
        
        public override void Initialize(Vector2Int worldCoord, int areaIdentity)
        {
            BodyParts.Add("TreeStump", TreeStump);
            BodyParts.Add("TreeBody", TreeBody);

            TreeStump.AttachBodyPart = TreeBody.Name;
            
            BodyParts.Add(TreeBody.Name, TreeBody);
            BodyParts.Add(TreeStump.Name, TreeStump);
            
            base.Initialize(worldCoord, areaIdentity);
        }
    }
}