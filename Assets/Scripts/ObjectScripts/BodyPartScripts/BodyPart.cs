using System;
using UtilScripts;
using UtilScripts.Text;
using Object = UnityEngine.Object;

namespace ObjectScripts.BodyPartScripts
{
    /// <inheritdoc cref="INamed" />
    /// <summary>
    ///     BodyPart of object; An object that not basic is made up by several body parts
    /// </summary>
    [Serializable]
    public class BodyPart : INamed
    {
        /// <summary>
        ///     The name of attached body part. If current components destroyed, attached component destroyed too
        /// </summary>
        public string AttachBodyPart;

        [NonSerialized] public bool Available = true;

        /// <summary>
        ///     Index of make-up components, which set in GameSetting
        /// </summary>
        public int ComponentIndex;

        /// <summary>
        ///     Defence of Cut Damage, real damage = CutDamage - CutDefence
        /// </summary>
        public float CutDefence = 10.0f;

        /// <summary>
        ///     If Cut Point reaches 0, the body part becomes unavailable and drop it components
        /// </summary>
        public LimitValue CutPoint = new LimitValue(10);

        public bool Fetchable;

        /// <summary>
        ///     the damage to the character if this part is destroyed
        /// </summary>
        public float Health = 20;

        /// <summary>
        ///     If Hit Point reaches 0, the body part becomes unavailable and drop it components(But in fact nothing should drop
        ///     because of the body part is destroyed completely)
        /// </summary>
        public LimitValue HitPoint = new LimitValue(10);

        /// <summary>
        ///     Defence ratio of Hit Damage, real damage = HitDamage * HitRatio
        /// </summary>
        public float HitRatio = 0.90f;

        public string Name;

        public PartPos PartPos = PartPos.Middle;

        [NonSerialized] public ComplexObject Self;

        public float Size = 10;
        public string TextName;
        public float Weight = 10;


        public string GetTextName()
        {
            return TextName;
        }


        public object Create(ComplexObject self)
        {
            return new BodyPart
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
                Health = Health,
                Fetchable = Fetchable,
                ComponentIndex = ComponentIndex,
                Self = self
            };
        }

        /// <summary>
        ///     Called when the body part received damage
        /// </summary>
        /// <param name="damage"></param>
        /// <returns>The intensity of these damage</returns>
        public float DoDamage(DamageValue damage)
        {
            if (!Available) return 0;
            var intensity = damage.DoDamage(this);
            if (HitPoint.Value > 0 && CutPoint.Value > 0) return intensity;
            Destroy();
            return intensity;
        }

        /// <summary>
        ///     Called when the body part received fixed hit damage
        /// </summary>
        /// <param name="damage"></param>
        public void DoPenetrateHitDamage(float damage)
        {
            HitPoint.Value -= damage;
            if (HitPoint.Value > 0) return;
            Destroy();
        }

        /// <summary>
        ///     Called when the body part is been destroyed(usually because of attach to a body part
        /// </summary>
        public void Destroy()
        {
            if (!Available) return;
            Available = false;

            if (Self == null) return;
            if (!string.IsNullOrEmpty(AttachBodyPart) &&
                Self.BodyParts.ContainsKey(AttachBodyPart))
                Self.BodyParts[AttachBodyPart].Destroy();

            if (ComponentIndex >= SceneManager.Instance.ComponentList.Count) return;
            foreach (var component in SceneManager.Instance.ComponentList[ComponentIndex].Components)
            {
                if (Utils.ProcessRandom.NextDouble() > HitPoint.GetRemainRatio()) continue;

                var instance = Object.Instantiate(component);
                instance.Info += GameText.Instance.GetPartInfoLog(Self.TextName, TextName);
                instance.Initialize(Utils.GetRandomShiftPosition(Self.WorldPos));
            }
        }
    }
}