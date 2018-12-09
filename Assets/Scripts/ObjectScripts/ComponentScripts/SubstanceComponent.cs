using System;
using UnityEngine;
using UtilScripts;

namespace ObjectScripts.ComponentScripts
{
    public class SubstanceComponent
    {
        public string Name;
        public LimitValue Durability = new LimitValue(100);
        public int Size = 10;
        public int Defence = 10;
        public bool Available = true;
        
        // Return: Whether the object should be destroyed
        public bool Damage(int damage, float defenceRatio = 1f)
        {
            Durability.Value -= Mathf.Max(0, damage - (int)(Defence * defenceRatio));
            if (Durability.Value > 0) return false;
            if (Essential) return true;   
            Available = false;
            if (AttachSubstanceComponent == null) return false;
            AttachSubstanceComponent.Damage(AttachSubstanceComponent.Durability.MaxValue, 0);
            return false;
        }

        // If current components destroyed, attached component destroyed too
        public SubstanceComponent AttachSubstanceComponent = null;
        
        // If essential is true, substance destroyed after the component destroyed
        public bool Essential;
    }
}