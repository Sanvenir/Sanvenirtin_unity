using System.Linq;
using AreaScripts;
using ExceptionScripts;
using UnityEngine;

namespace ObjectScripts
{
    public class Substance: BasicObject
    {
        public Vector2 GlobalPos;
        public Vector2Int GlobalCoord;
        public SubstanceProperties SubstanceProperties;
        public LayerMask BlockLayer;

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
    }
}