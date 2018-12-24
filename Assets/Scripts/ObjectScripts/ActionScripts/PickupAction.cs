using ObjectScripts.CharSubstance;

namespace ObjectScripts.ActionScripts
{
    public class PickupAction: BaseAction
    {
        public PickupAction(Character self) : base(self)
        {
        }

        public override void DoAction(bool check = true)
        {
        }

        public override bool CheckAction()
        {
            return true;
        }
    }
}