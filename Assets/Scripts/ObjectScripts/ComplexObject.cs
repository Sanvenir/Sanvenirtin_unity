using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AreaScripts;
using ExceptionScripts;
using ObjectScripts.BodyPartScripts;
using ObjectScripts.SpriteController;
using SpriteGlow;
using UnityEditor;
using UnityEngine;
using UtilScripts;
using UtilScripts.Text;
using BodyPart = ObjectScripts.BodyPartScripts.BodyPart;

namespace ObjectScripts
{
    /// <inheritdoc />
    /// <summary>
    /// Object which consists of several body parts
    /// </summary>
    public class ComplexObject : BaseObject
    {
        
        [NonSerialized]
        public Dictionary<string, BodyPart> BodyParts = new Dictionary<string, BodyPart>();

        [HideInInspector] public bool IsDestroy = false;
        
        private readonly List<BodyPart> _unreachableParts = new List<BodyPart>();

        private readonly List<BodyPart> _highParts = new List<BodyPart>();

        private readonly List<BodyPart> _middleParts = new List<BodyPart>();

        private readonly List<BodyPart> _lowParts = new List<BodyPart>();

        public virtual float Attacked(DamageValue damage, BodyPart bodyPart)
        {
            if (bodyPart == null)
                return 0;
            var intensity = bodyPart.DoDamage(damage);
            
            if (BodyParts.Values.Any(part => part.Available))
            {
                return intensity;
            }

            IsDestroy = true;
            return intensity;
        }

        public List<BodyPart> GetBodyParts(PartPos partPos)
        {
            switch (partPos)
            {
                case PartPos.Unreachable:
                    return _unreachableParts;
                case PartPos.High:
                    return _highParts;
                case PartPos.Middle:
                    return _middleParts;
                case PartPos.Low:
                    return _lowParts;
                case PartPos.Arbitrary:
                    return BodyParts.Values.ToList();
                default:
                    throw new ArgumentOutOfRangeException("partPos", partPos, null);
            }
        }

        public virtual void Initialize(Vector2Int worldCoord, int areaIdentity)
        {
            base.Initialize(SceneManager.Instance.WorldCoordToPos(worldCoord));
            foreach (var part in BodyParts.Values)
            {
                part.HitPoint.Value = part.HitPoint.MaxValue;
                switch (part.PartPos)
                {
                    case PartPos.Unreachable:
                        _unreachableParts.Add(part);
                        break;
                    case PartPos.Arbitrary:
                        _unreachableParts.Add(part);
                        _highParts.Add(part);
                        _middleParts.Add(part);
                        _lowParts.Add(part);
                        break;
                    case PartPos.High:
                        _highParts.Add(part);
                        break;
                    case PartPos.Middle:
                        _middleParts.Add(part);
                        break;
                    case PartPos.Low:
                        _lowParts.Add(part);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        protected override void Update()
        {
            base.Update();
            if (!IsDestroy) return;
            SceneManager.Instance.Print(
                GameText.Instance.GetSubstanceDestroyLog(TextName), WorldCoord);
            foreach (var bodyPart in BodyParts.Values)
            {
                bodyPart.Destroy();
            }
            Destroy(gameObject);
        }

        public override float GetSize()
        {
            return Collider2D.bounds.size.magnitude;
        }

        private float _weight = 0;
        public override float GetWeight()
        {
            _weight = 0;
            foreach (var part in BodyParts.Values)
            {
                if (part.Available)
                {
                    _weight += part.Weight;
                }
            }

            return _weight;
        }
    }
}