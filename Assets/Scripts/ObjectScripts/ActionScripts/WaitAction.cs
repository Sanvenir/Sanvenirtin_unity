using ObjectScripts.CharSubstance;

namespace ObjectScripts.ActionScripts
{
    public class WaitAction: BaseAction
    {
        
        public WaitAction(Character self) : base(self)
        {
            CostTime = Self.Properties.GetReactTime();
        }
    }
}