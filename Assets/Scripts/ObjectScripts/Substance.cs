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
    public class Substance: BasicObject
    {
        public Vector2 WorldPos;
        public Vector2Int WorldCoord;
        public LayerMask BlockLayer;

        public Dictionary<string, SubstanceComponent> Components = new Dictionary<string, SubstanceComponent>();

        public bool IsDestroy = false;

        // Component Key represent the place of this component
        public virtual void Attacked(int damage, string componentKey, float defenceRatio=1f)
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
            MoveTo(worldCoord);
        }

        public void MoveTo(Vector2Int worldCoord)
        {
            WorldCoord = worldCoord;
            WorldPos = SceneManager.Instance.WorldCoordToPos(worldCoord);
            var hit = Physics2D.OverlapPoint(WorldPos, BlockLayer);
            SpriteRenderer.sortingOrder = -worldCoord.y;
            transform.position = WorldPos;
            if (hit != null)
            {
                throw new CoordOccupiedException(hit);
            }
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
            try
            {
                AreaIdentity = SceneManager.Instance.WorldCoordToArea(WorldCoord).Identity;
            }
            catch (AreaNotFoundCondition e)
            {
                // Object out of the activated areas
                Destroy(gameObject);
                // Debug.Log("Destroying " + gameObject.Name);
            }
        }

        public Collider2D GetColliderAtWorldCoord(Vector2Int coord)
        {
            return Physics2D.OverlapPoint(
                SceneManager.Instance.WorldCoordToPos(coord), BlockLayer);
        }

        private void Update()
        {
            if (IsDestroy)
            {
                Destroy(gameObject);
                
                //TODO: Add dropping objects here
            }
        }
    }
}