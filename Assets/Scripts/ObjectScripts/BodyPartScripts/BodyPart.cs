using System;
using System.Collections.Generic;
using UnityEngine;
using UtilScripts;
using UtilScripts.Text;
using Object = UnityEngine.Object;

namespace ObjectScripts.BodyPartScripts
{
    /// <inheritdoc cref="INamed" />
    /// <summary>
    /// BodyPart of object; An object that not basic is made up by several body parts
    /// </summary>
    [Serializable]
    public class BodyPart: INamed, ICloneable
    {
        private const float DropIncrement = 0.1f;
        public string Name;
        public string TextName;
        public LimitValue CutPoint = new LimitValue(1000);
        public LimitValue HitPoint = new LimitValue(1000);
        public float Size = 10;
        public float Weight = 10;

        public float CutDefence = 10.0f;
        public float HitRatio = 0.90f;
        
        // public float Defence = 10;
        [NonSerialized]
        public bool Available = true;
        
        public PartPos PartPos = PartPos.Middle;
        // If current components destroyed, attached component destroyed too
        public string AttachBodyPart;

        // If essential is true, substance destroyed after the component destroyed
        public bool Essential;
        public bool Fetchable;

        public int ComponentIndex;
        

        public object Clone()
        {
            return new BodyPart()
            {
                Name = Name,
                TextName = TextName,
                HitPoint = HitPoint,
                CutPoint = CutPoint,
                Size = Size,
                Weight = Weight,
                CutDefence = CutDefence,
                HitRatio = HitRatio,
                Available = Available,
                PartPos = PartPos,
                AttachBodyPart = AttachBodyPart,
                Essential = Essential,
                Fetchable = Fetchable,
                ComponentIndex = ComponentIndex,
                ComplexObject = ComplexObject
            };
        }
        
        [NonSerialized]
        public ComplexObject ComplexObject;

        // Return: The intensity of these damage
        public float DoDamage(DamageValue damage, float defenceRatio = 1f)
        {
            if (!Available) return 0;
            var intensity = damage.DoDamage(this);
            if (HitPoint.Value > 0 && CutPoint.Value > 0) return intensity;
            Destroy();
            return intensity;
        }

        public void DoPenetrateHitDamage(float damage)
        {
            HitPoint.Value -= damage;
            if (HitPoint.Value > 0) return;
            Destroy();
        }

        public void Destroy()
        {
            SceneManager.Instance.Print(
                GameText.Instance.GetBodyPartDestroyLog(
                    ComplexObject.TextName, TextName));
            Available = false;

            if (!string.IsNullOrEmpty(AttachBodyPart) &&
                ComplexObject.BodyParts.ContainsKey(AttachBodyPart)) 
                ComplexObject.BodyParts[AttachBodyPart].Destroy();
            
            if(ComponentIndex >= GameSetting.Instance.ComponentList.Count) return;
            foreach (var component in GameSetting.Instance.ComponentList[ComponentIndex].Components)
            {
                if (Utils.ProcessRandom.NextDouble() > HitPoint.GetRemainRatio())
                {
                    continue;
                }

                Object.Instantiate(component).transform.position =
                    ComplexObject.WorldPos + new Vector2(
                        (float) Utils.ProcessRandom.NextDouble() * DropIncrement * 2 - DropIncrement,
                        (float) Utils.ProcessRandom.NextDouble() * DropIncrement * 2 - DropIncrement);
            }
        }

        
        public string GetTextName()
        {
            return TextName;
        }
    }
}