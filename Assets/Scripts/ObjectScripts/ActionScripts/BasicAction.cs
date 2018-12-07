using System;

namespace ObjectScripts.ActionScripts
{
    public abstract class BasicAction
    {
        public Character Self;

        // Return Cost Time
        public abstract int DoAction();
    }
}