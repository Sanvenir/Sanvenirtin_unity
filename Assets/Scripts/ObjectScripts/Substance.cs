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
            MoveTo(globalCoord);
            AreaIdentity = areaIdentity;
        }

        public void MoveTo(Vector2Int globalCoord)
        {
            GlobalCoord = globalCoord;
            GlobalPos = SceneManager.Instance.GlobalCoordToPos(globalCoord);
            if (Physics2D.OverlapPoint(GlobalPos, BlockLayer))
            {
                throw new GameException("Moving Substance to an occupied position");
            }

            transform.position = GlobalPos;
            SpriteRenderer.sortingOrder = -globalCoord.y;
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
            }
        }

        public Collider2D GetColliderAtGlobalCoord(Vector2Int coord)
        {
            return Physics2D.OverlapPoint(
                SceneManager.Instance.GlobalCoordToPos(coord), BlockLayer);
        }
    }
}