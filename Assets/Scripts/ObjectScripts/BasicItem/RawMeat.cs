using ObjectScripts.CharSubstance;

namespace ObjectScripts.BasicItem
{
    public class RawMeat: BasicItem
    {
        public override void Eat(Character character)
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