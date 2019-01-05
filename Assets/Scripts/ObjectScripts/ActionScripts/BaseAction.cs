using System;
using ObjectScripts.CharSubstance;
using UnityEngine;
using UtilScripts;

namespace ObjectScripts.ActionScripts
{
    public abstract class BaseAction
    {
        protected readonly Character Self;
        
        protected int CostTime;

        public BaseAction(Character self)
        {
            Self = self;
        }
        
        // Return Cost Time
        public virtual bool DoAction()
        {
            Self.ActivateTime += CostTime;
            return true;
        }
    }
}