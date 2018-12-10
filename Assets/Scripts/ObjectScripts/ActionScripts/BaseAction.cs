using System;
using UnityEngine;
using UtilScripts;

namespace ObjectScripts.ActionScripts
{
    public abstract class BaseAction
    {
        protected readonly Character Self;

        protected int CostTime;
        public Direction TargetDirection;
        public Vector2Int TargetCoord;

        public BaseAction(Character self)
        {
            Self = self;
        }
        
        // Return Cost Time
        public abstract void DoAction(bool check = true);
        public abstract bool CheckAction();
    }
}