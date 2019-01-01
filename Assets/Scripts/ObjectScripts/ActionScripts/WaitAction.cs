using ObjectScripts.CharSubstance;

namespace ObjectScripts.ActionScripts
{
    public class WaitAction: BaseAction
    {
        public override void DoAction(bool check = true)
        {
            Self.ActivateTime += Self.Properties.GetReactTime();
        }

        public override bool CheckAction()
        {
            return true;
        }

        public WaitAction(Character self) : base(self)
        {
        }
    }
}