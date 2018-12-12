using System.Collections.Generic;
using System.Linq;
using AreaScripts;
using Boo.Lang;
using ExceptionScripts;
using ObjectScripts.ComponentScripts;
using UnityEngine;
using UtilScripts;

namespace ObjectScripts
{
    public class Substance : BaseObject
    {
        public Vector2 WorldPos;
        public Vector2Int WorldCoord;
        public ContactFilter2D ContactFilter;

        public Dictionary<string, SubstanceComponent> Components = new Dictionary<string, SubstanceComponent>();

        public bool IsDestroy = false;

        // Component Key represent the place of this component
        public virtual void Attacked(int damage, string componentKey, float defenceRatio = 1f)
        {
            if (!Components.ContainsKey(componentKey))
            {
                return;
            }

            if (!Components[componentKey].Damage(damage, defenceRatio))
                return;

            IsDestroy = true;
        }

        // Read Only
        public int AreaIdentity;

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