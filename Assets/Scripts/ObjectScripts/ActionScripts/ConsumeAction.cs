using ObjectScripts.BasicItem;
using ObjectScripts.CharSubstance;

namespace ObjectScripts.ActionScripts
{
    public class ConsumeAction: BaseAction
    {
        private readonly IConsumeItem _item;
        
        public ConsumeAction(Character self, IConsumeItem item) : base(self)
        {
            _item = item;
            CostTime = Self.Properties.GetActTime();
        }

        public override bool DoAction()
        {
            base.DoAction();
            _item.DoConsume(Self);
            return true;
        }
    }
}