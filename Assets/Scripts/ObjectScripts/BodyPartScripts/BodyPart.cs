using System;
using System.Collections.Generic;
using UnityEngine;
using UtilScripts;
using UtilScripts.Text;
using Object = UnityEngine.Object;

namespace ObjectScripts.BodyPartScripts
{
    [Serializable]
    public class BodyPart: INamed, ICloneable
    {
        public const float DropIncrement = 0.1f;
        public string Name;
        public string TextName;
        public LimitValue HitPoint = new LimitValue(1000);
        public float Size = 10;
        public float Weight = 10;
        public float Defence = 10;
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
                Size = Size,
                Weight = Weight,
                Defence = Defence,
                Available = Available,
                PartPos = PartPos,
                AttachBodyPart = AttachBodyPart,
                Essential = Essential,
                Fetchable = Fetchable,
                ComponentIndex = ComponentIndex,
                Substance = Substance
            };
        }
        
        [NonSerialized]
        public Substance Substance;

        // Return: Whether the object should be destroyed
        public float Damage(float damage, float defenceRatio = 1f)
        {
            if (!Available) return damage;
            damage = Mathf.Max(0f, damage - (Defence * defenceRatio));
            HitPoint.Value -= damage;
            if (HitPoint.Value > 0) return damage;
            Destroy();
            return damage;
        }

        public void Destroy()
        {
            SceneManager.Instance.Print(
                GameText.Instance.GetBodyPartDestroyLog(
                    Substance.TextName, TextName));
            Available = false;

            if (AttachBodyPart != string.Empty &&
                Substance.BodyParts.ContainsKey(AttachBodyPart)) 
                Substance.BodyParts[AttachBodyPart].Destroy();
            
            if(ComponentIndex >= GameSetting.Instance.ComponentList.Count) return;
            foreach (var component in GameSetting.Instance.ComponentList[ComponentIndex].Components)
            {
                if (Utils.ProcessRandom.NextDouble() > HitPoint.GetRemainRatio())
                {
                    continue;
                }

                Object.Instantiate(component).transform.position =
                    Substance.WorldPos + new Vector2(
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