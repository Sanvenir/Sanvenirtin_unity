using ObjectScripts.CharSubstance;
using UtilScripts;

namespace ObjectScripts.StyleScripts
{
    public abstract class BaseStyle : INamed
    {
        public Character Self;

        public BaseStyle(Character self)
        {
            Self = self;
        }

        public abstract string GetTextName();
    }
}