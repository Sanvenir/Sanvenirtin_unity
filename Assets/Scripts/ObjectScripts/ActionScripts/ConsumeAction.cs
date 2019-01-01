using ObjectScripts.BasicItem;
using ObjectScripts.CharSubstance;

namespace ObjectScripts.ActionScripts
{
    public class ConsumeAction: BaseAction
    {
        private IConsumeItem _item;
        
        public ConsumeAction(Character self, IConsumeItem item) : base(self)
        {
            _item = item;
        }

        public override void DoAction(bool check = true)
        {
            CostTime = Self.Properties.GetActTime();
            Self.ActivateTime += CostTime;
            _item.DoConsume(Self);
        }

        public override bool CheckAction()
        {
            return true;
        }
    }
}