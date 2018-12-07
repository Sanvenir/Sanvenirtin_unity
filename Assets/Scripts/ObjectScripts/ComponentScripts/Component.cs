using System;
using UtilScripts;

namespace ObjectScripts.ComponentScripts
{
    [Serializable]
    public class Component
    {
        public LimitValue Durability = new LimitValue(100);
        public int Size = 10;
        public int Strength = 10;
        public int Dexterity = 10;
        public int Defence = 10;
        public bool Available = true;

        public int UseStrength()
        {
            return Available ? Strength : 0;
        }

        public int UseDexterity()
        {
            return Available ? Dexterity : 0;
        }

        // Return: Whether the object should be destroyed
        public bool Damage(int damage)
        {
            Durability.Value -= damage;
            if (Durability.Value > 0) return false;
            if (Essential) return true;   
            Available = false;
            return false;
        }

        // If current components destroyed, all attached components destroyed too
        public Component AttachComponent;
        
        // If essential is true, substance destroyed after the component destroyed
        public bool Essential;
    }
}