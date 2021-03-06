using System;
using ObjectScripts.BodyPartScripts;
using UnityEngine;

namespace UtilScripts
{
    [Serializable]
    public struct DamageValue
    {
        public const float HitDamageRatio = 0.1f;

        // Cut Damage can make the body part drop, and affected by absolute defend value <CutDefence>
        public float CutDamage;

        // Hit Damage can destroy the body part, and affected by multiply by a decreasing ratio <HitRatio>
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

        public static DamageValue operator +(DamageValue damageValue1, DamageValue damageValue2)
        {
            return new DamageValue(
                damageValue1.CutDamage + damageValue2.CutDamage,
                damageValue2.HitDamage + damageValue2.HitDamage);
        }

        public static DamageValue operator *(DamageValue damageValue, float ratio)
        {
            return new DamageValue(damageValue.CutDamage * ratio, damageValue.CutDamage * ratio);
        }
    }
}