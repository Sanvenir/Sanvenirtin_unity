using System;
using UnityEngine;
using UtilScripts;

namespace ObjectScripts.BodyPartScripts
{
    [Serializable]
    public class BodyPart
    {
        public string Name;
        public LimitValue Durability = new LimitValue(1000);
        public float Size = 10;
        public float Weight = 10;
        public float Defence = 10;
        public bool Available = true;
        
        // Return: Whether the object should be destroyed
        public float Damage(float damage, float defenceRatio = 1f)
        {
            damage = Mathf.Max(0f, damage - (Defence * defenceRatio));
            Durability.Value -= damage;
            if (Durability.Value > 0) return damage;   
            Available = false;
            if (AttachBodyPart == null) return 0;
            AttachBodyPart.Damage(AttachBodyPart.Durability.MaxValue, 0);
            return 0;
        }

        // If current components destroyed, attached component destroyed too
        public BodyPart AttachBodyPart = null;
        
        // If essential is true, substance destroyed after the component destroyed
        public bool Essential;
    }
}