using System;
using System.Collections;
using System.Collections.Generic;
using AreaScripts;
using ExceptionScripts;
using ObjectScripts.BodyPartScripts;
using UnityEditor;
using UnityEngine;
using UtilScripts;
using BodyPart = ObjectScripts.BodyPartScripts.BodyPart;

namespace ObjectScripts
{
//    [CustomEditor(typeof(Substance))]
//    public class SubstanceEditor : Editor
//    {
//        public override void OnInspectorGUI()
//        {
//            base.OnInspectorGUI();
//            var substance = (Substance) target;
//            foreach (var part in substance.BodyParts.Values)
//            {
//                part.Defence = EditorGUILayout.IntField("Defense", part.Defence);
//               
//            }
//        }
//    }
    public class Substance : BaseObject
    {
        [HideInInspector] public Vector2 WorldPos;
        [HideInInspector] public Vector2Int WorldCoord;

        public ContactFilter2D ContactFilter;

        // public SortedList<string, BodyPart> BodyParts = new SortedList<string, BodyPart>();
        public List<BodyPart> MidairParts = new List<BodyPart>();
        public List<BodyPart> HighParts = new List<BodyPart>();
        public List<BodyPart> MiddleParts = new List<BodyPart>();
        public List<BodyPart> LowParts = new List<BodyPart>();

        [HideInInspector] public bool IsDestroy = false;

        public virtual void Attacked(int damage, BodyPart bodyPart, float defenceRatio = 1f)
        {
            if (bodyPart == null)
                return;
            if (!bodyPart.Damage(damage, defenceRatio))
                return;
            IsDestroy = true;
        }

        // Read Only
        public int AreaIdentity;

        public IEnumerable<BodyPart> GetAllBodyParts()
        {
            foreach (var part in MidairParts)
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
                    return MidairParts;
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
            AreaIdentity = areaIdentity;
            ContactFilter.useLayerMask = true;
            MoveTo(worldCoord);
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

        public Substance GetColliderAtWorldCoord(Vector2Int coord)
        {
            Collider2D.offset = coord - WorldCoord;
            var result = CheckCollider<Substance>();
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

        public T CheckCollider<T>()
            where T : Substance
        {
            var colliders = new Collider2D[1];
            Collider2D.OverlapCollider(ContactFilter, colliders);
            return colliders[0] == null
                ? null
                : colliders[0].GetComponent<T>();
        }
    }
}