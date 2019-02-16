using ObjectScripts.ItemScripts;
using ObjectScripts.CharSubstance;

namespace ObjectScripts.ActionScripts
{
    /// <inheritdoc />
    /// <summary>
    ///     An action make character consume an item
    /// </summary>
    public class ConsumeAction : BaseAction
    {
        private readonly IConsumableItem _item;

        public ConsumeAction(Character self, IConsumableItem item) : base(self)
        {
            _item = item;
            CostTime = Self.Properties.GetActTime(0);
        }

        public override bool DoAction()
        {
            _item.DoConsume(Self);
            return true;
        }
    }
}