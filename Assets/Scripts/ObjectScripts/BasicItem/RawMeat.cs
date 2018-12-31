using ObjectScripts.CharSubstance;

namespace ObjectScripts.BasicItem
{
    public class RawMeat: BasicItem, IConsumeItem
    {
        public void DoConsume(Character character)
        {
            Weight -= 1;
            character.Hunger -= 10;
            if (Weight <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}