using System;

namespace ObjectScripts.ActionScripts
{
    public abstract class BasicAction
    {
        protected readonly Character Self;

        protected int CostTime;

        public BasicAction(Character self)
        {
            Self = self;
        }
        
        // Return Cost Time
        public abstract void DoAction(bool check = true);
        public abstract bool CheckAction();
    }
}