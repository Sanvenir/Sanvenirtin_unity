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
    public class ComplexObject : BaseObject
    {
        [HideInInspector] public Vector2 WorldPos;
        [HideInInspector] public Vector2Int WorldCoord;
        
        public SpriteController.SpriteController SpriteController;

        // public SortedList<string, BodyPart> BodyParts = new SortedList<string, BodyPart>();
        private readonly List<BodyPart> _unreachableParts = new List<BodyPart>();

        private readonly List<BodyPart> _highParts = new List<BodyPart>();

        private readonly List<BodyPart> _middleParts = new List<BodyPart>();

        private readonly List<BodyPart> _lowParts = new List<BodyPart>();
        
        [NonSerialized]
        public Dictionary<string, BodyPart> BodyParts = new Dictionary<string, BodyPart>();

        [HideInInspector] public bool IsDestroy = false;

        public void SetDisable()
        {
            SpriteController.SetDisable(true);
            gameObject.layer = LayerMask.NameToLayer("ItemLayer");
        }

        public virtual float Attacked(DamageValue damage, BodyPart bodyPart)
        {
            if (bodyPart == null)
                return 0;
            var intensity = bodyPart.DoDamage(damage);

            if (bodyPart.Essential && !bodyPart.Available)
            {
                SetDisable();
            }
            
            if (BodyParts.Values.Any(part => part.Available))
            {
                return intensity;
            }

            IsDestroy = true;
            return intensity;
        }

        // Read Only
        [HideInInspector]
        public int AreaIdentity;

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
            base.Initialize();
            gameObject.layer = LayerMask.NameToLayer("BlockLayer");
            AreaIdentity = areaIdentity;
            MoveTo(worldCoord);
            foreach (var part in BodyParts.Values)
            {
                part.HitPoint.Value = part.HitPoint.MaxValue;
                part.ComplexObject = this;
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

        public void MoveTo(Vector2Int worldCoord)
        {
            WorldCoord = worldCoord;
            WorldPos = SceneManager.Instance.WorldCoordToPos(worldCoord);
            SpriteRenderer.sortingOrder = -worldCoord.y;
            transform.position = WorldPos;
        }

        private void LateUpdate()
        {
            if (!SceneManager.Instance.ActivateAreas.ContainsKey(AreaIdentity))
            {
                Destroy(gameObject);
                return;
            }

            var area = SceneManager.Instance.ActivateAreas[AreaIdentity];
            if (area.IsWorldCoordInsideArea(WorldCoord)) return;
            area = SceneManager.Instance.WorldPosToArea(WorldPos);
            if (area == null)
            {
                Destroy(gameObject);
                return;
            }

            AreaIdentity = area.Identity;
        }

        public bool CheckColliderAtWorldCoord(
            Vector2Int coord, out ComplexObject complexObject)
        {
            Collider2D.offset = coord - WorldCoord;
            var result = CheckCollider(out complexObject);
            Collider2D.offset = Vector2.zero;
            return result;
        }

        public bool CheckColliderAtWorldCoord(Vector2Int coord)
        {
            Collider2D.offset = coord - WorldCoord;
            var result = CheckCollider();
            Collider2D.offset = Vector2.zero;
            return result;
        }

        protected virtual void Update()
        {
            if (IsDestroy)
            {
                SceneManager.Instance.Print(
                    GameText.Instance.GetSubstanceDestroyLog(TextName));
                Destroy(gameObject);

                //TODO: Add dropping objects here
            }
        }

        public bool CheckCollider<T>(out T collide)
            where T : ComplexObject
        {
            var colliders = new Collider2D[1];
            Collider2D.OverlapCollider(SceneManager.Instance.BlockFilter, colliders);
            collide = colliders[0] == null
                ? null
                : colliders[0].GetComponent<T>();
            return colliders[0] == null;
        }
        
        public bool CheckCollider()
        {
            var colliders = new Collider2D[1];
            Collider2D.OverlapCollider(SceneManager.Instance.BlockFilter, colliders);
            return colliders[0] == null;
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