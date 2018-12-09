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
        public Vector2 GlobalPos;
        public Vector2Int GlobalCoord;
        public LayerMask BlockLayer;

        public Dictionary<string, SubstanceComponent> Components = new Dictionary<string, SubstanceComponent>();

        public bool IsDestroy = false;

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

        public virtual void Initialize(Vector2Int globalCoord, int areaIdentity)
        {
            base.Initialize();
            AreaIdentity = areaIdentity;
            MoveTo(globalCoord);
        }

        public void MoveTo(Vector2Int globalCoord)
        {
            GlobalCoord = globalCoord;
            GlobalPos = SceneManager.Instance.GlobalCoordToPos(globalCoord);
            var hit = Physics2D.OverlapPoint(GlobalPos, BlockLayer);
            SpriteRenderer.sortingOrder = -globalCoord.y;
            transform.position = GlobalPos;
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
            if (area.IsGlobalCoordInsideArea(GlobalCoord)) return;
            try
            {
                AreaIdentity = SceneManager.Instance.GlobalCoordToArea(GlobalCoord).Identity;
            }
            catch (AreaNotFoundCondition e)
            {
                // Object out of the activated areas
                Destroy(gameObject);
                // Debug.Log("Destroying " + gameObject.name);
            }
        }

        public Collider2D GetColliderAtGlobalCoord(Vector2Int coord)
        {
            return Physics2D.OverlapPoint(
                SceneManager.Instance.GlobalCoordToPos(coord), BlockLayer);
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