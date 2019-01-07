using ObjectScripts.CharSubstance;

namespace ObjectScripts.ItemScripts
{
    /// <summary>
    ///     Item which can be consumed
    /// </summary>
    public interface IConsumableItem
    {
        void DoConsume(Character character);
    }
}