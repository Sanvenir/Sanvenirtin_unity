using UnityEngine;

namespace ObjectScripts.ActionScripts
{
    public abstract class CoordinateAction: BasicAction
    {
        public Vector2Int TargetCoord;

        protected CoordinateAction(Character self) : base(self)
        {
        }
    }
}