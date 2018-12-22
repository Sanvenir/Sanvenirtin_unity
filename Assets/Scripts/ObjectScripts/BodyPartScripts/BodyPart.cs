using System;
using System.Collections.Generic;
using UnityEngine;
using UtilScripts;
using Object = UnityEngine.Object;

namespace ObjectScripts.BodyPartScripts
{
    [Serializable]
    public class BodyPart
    {
        public string Name;
        public LimitValue HitPoint = new LimitValue(1000);
        public float Size = 10;
        public float Weight = 10;
        public float Defence = 10;
        public bool Available = true;
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
            SceneManager.Instance.Print(Substance.Name + "'s " + Name + " is destroyed!");
            Available = false;
            foreach (var component in Components)
            {
                if (Utils.ProcessRandom.NextDouble() > HitPoint.GetRemainRatio())
                {
                    continue;
                }

                Object.Instantiate(component);
                component.transform.position =
                    Substance.transform.position + new Vector3(
                        (float) Utils.ProcessRandom.NextDouble() - 0.5f,
                        (float) Utils.ProcessRandom.NextDouble() - 0.5f, 0);
            }

            if (AttachBodyPart == null) return;
            AttachBodyPart.Destroy();
        }

        // If current components destroyed, attached component destroyed too
        public BodyPart AttachBodyPart = null;

        // If essential is true, substance destroyed after the component destroyed
        public bool Essential;

        public List<BasicItem.BasicItem> Components;
    }
}