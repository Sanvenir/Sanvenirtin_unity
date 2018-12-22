using ObjectScripts.CharSubstance;

namespace ObjectScripts.BasicItem
{
    public abstract class BasicItem: BaseObject
    {
        public int Weight;
        public int Size;

        public override float GetSize()
        {
            return Size;
        }

        public override float GetWeight()
        {
            return Weight;
        }

        public abstract void Eat(Character character);
    }
}