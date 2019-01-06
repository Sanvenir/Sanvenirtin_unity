using System;
using ObjectScripts.BodyPartScripts;
using UnityEngine;

namespace UtilScripts
{
    [Serializable]
    public struct DamageValue
    {
        public const float HitDamageRatio = 0.1f;
        
        public float CutDamage;
        public float HitDamage;
        
        public DamageValue(
            float cutDamage = 0f, 
            float hitDamage = 0f)
        {
            CutDamage = cutDamage;
            HitDamage = hitDamage;
        }

        public float DoDamage(BodyPart bodyPart)
        {
            var hitDamage = HitDamage * bodyPart.HitRatio;
            var cutDamage = Mathf.Max(CutDamage - bodyPart.CutDefence, 0f);
            bodyPart.HitPoint.Value -= hitDamage;
            bodyPart.CutPoint.Value -= cutDamage;
            return hitDamage + cutDamage;
        }
    }
}