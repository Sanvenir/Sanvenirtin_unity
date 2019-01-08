using System;
using UnityEngine;
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

        public string Name;
        public string TextName;

        /// <summary>
        ///     If Cut Point reaches 0, the body part becomes unavailable and drop it components
        /// </summary>
        public LimitValue CutPoint = new LimitValue(10);

        /// <summary>
        ///     If Hit Point reaches 0, the body part becomes unavailable and drop it components(But in fact nothing should drop
        ///     because of the body part is destroyed completely)
        /// </summary>
        public LimitValue HitPoint = new LimitValue(10);

        public float Size = 10;
        public float Weight = 10;

        /// <summary>
        ///     Defence of Cut Damage, real damage = CutDamage - CutDefence
        /// </summary>
        public float CutDefence = 10.0f;

        /// <summary>
        ///     Defence ratio of Hit Damage, real damage = HitDamage * HitRatio
        /// </summary>
        public float HitRatio = 0.90f;

        [NonSerialized] public bool Available = true;

        public PartPos PartPos = PartPos.Middle;

        /// <summary>
        ///     The name of attached body part. If current components destroyed, attached component destroyed too
        /// </summary>
        public string AttachBodyPart;

        /// <summary>
        ///     If essential is true, character and substance destroyed after the component destroyed
        /// </summary>
        public bool Essential;

        public bool Fetchable;

        /// <summary>
        ///     Index of make-up components, which set in GameSetting
        /// </summary>
        public int ComponentIndex;


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
                Essential = Essential,
                Fetchable = Fetchable,
                ComponentIndex = ComponentIndex,
                Self = self
            };
        }

        [NonSerialized] public ComplexObject Self;

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
            SceneManager.Instance.Print(
                GameText.Instance.GetBodyPartDestroyLog(
                    Self.TextName, TextName), Self.WorldCoord);
            Available = false;

            if (!string.IsNullOrEmpty(AttachBodyPart) &&
                Self.BodyParts.ContainsKey(AttachBodyPart))
                Self.BodyParts[AttachBodyPart].Destroy();

            if (ComponentIndex >= SceneManager.Instance.ComponentList.Count) return;
            foreach (var component in SceneManager.Instance.ComponentList[ComponentIndex].Components)
            {
                if (Utils.ProcessRandom.NextDouble() > HitPoint.GetRemainRatio()) continue;

                var instance = Object.Instantiate(component);
                instance.SetPosition(Utils.GetRandomShiftPosition(Self.WorldPos));
                instance.Initialize();
            }
        }


        public string GetTextName()
        {
            return TextName;
        }
    }
}