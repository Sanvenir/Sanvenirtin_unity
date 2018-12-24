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
using BodyPart = ObjectScripts.BodyPartScripts.BodyPart;

namespace ObjectScripts
{
    public class Substance : BaseObject
    {
        [HideInInspector] public Vector2 WorldPos;
        [HideInInspector] public Vector2Int WorldCoord;
        
        public SpriteController.SpriteController SpriteController;

        // public SortedList<string, BodyPart> BodyParts = new SortedList<string, BodyPart>();
        [HideInInspector]
        public List<BodyPart> AirParts = new List<BodyPart>();
        [HideInInspector]
        public List<BodyPart> HighParts = new List<BodyPart>();
        [HideInInspector]
        public List<BodyPart> MiddleParts = new List<BodyPart>();
        [HideInInspector]
        public List<BodyPart> LowParts = new List<BodyPart>();
        public Dictionary<string, BodyPart> BodyParts = new Dictionary<string, BodyPart>();

        [HideInInspector] public bool IsDestroy = false;

        public void SetDisable()
        {
            SpriteController.SetDisable(true);
            gameObject.layer = LayerMask.NameToLayer("ItemLayer");
        }

        public virtual float Attacked(float damage, BodyPart bodyPart, float defenceRatio = 1f)
        {
            if (bodyPart == null)
                return 0;
            damage = bodyPart.Damage(damage, defenceRatio);

            if (bodyPart.Essential && !bodyPart.Available)
            {
                SetDisable();
            }
            
            if (GetAllBodyParts().Any(part => part.Available))
            {
                return damage;
            }

            IsDestroy = true;
            return damage;
        }

        // Read Only
        [HideInInspector]
        public int AreaIdentity;

        public IEnumerable<BodyPart> GetAllBodyParts()
        {
            foreach (var part in AirParts)
            {
                yield return part;
            }

            foreach (var part in HighParts)
            {
                yield return part;
            }

            foreach (var part in MiddleParts)
            {
                yield return part;
            }

            foreach (var part in LowParts)
            {
                yield return part;
            }
        }

        public List<BodyPart> GetBodyParts(PartPos partPos)
        {
            switch (partPos)
            {
                case PartPos.Midair:
                    return AirParts;
                case PartPos.High:
                    return HighParts;
                case PartPos.Middle:
                    return MiddleParts;
                case PartPos.Low:
                    return LowParts;
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
            foreach (var part in GetAllBodyParts())
            {
                part.HitPoint.Value = part.HitPoint.MaxValue;
                part.Substance = this;
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
            Vector2Int coord, out Substance substance)
        {
            Collider2D.offset = coord - WorldCoord;
            var result = CheckCollider(out substance);
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

        private void Update()
        {
            if (IsDestroy)
            {
                Destroy(gameObject);

                //TODO: Add dropping objects here
            }
        }

        public bool CheckCollider<T>(out T collide)
            where T : Substance
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
            foreach (var part in GetAllBodyParts())
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