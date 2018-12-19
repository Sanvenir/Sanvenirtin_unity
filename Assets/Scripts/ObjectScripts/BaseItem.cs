using UtilScripts;

namespace ObjectScripts
{
    public class BaseItem: BaseObject
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
    }
}