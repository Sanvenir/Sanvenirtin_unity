using ObjectScripts.CharSubstance;

namespace ObjectScripts.ActionScripts
{
    /// <inheritdoc />
    /// <summary>
    /// An action makes character wait for next turn
    /// </summary>
    public class WaitAction: BaseAction
    {
        
        public WaitAction(Character self) : base(self)
        {
            CostTime = Self.Properties.GetReactTime(0);
        }
    }
}