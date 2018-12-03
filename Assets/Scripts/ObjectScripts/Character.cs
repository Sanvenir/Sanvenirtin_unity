using System.Collections;
using AreaScripts;
using UnityEngine;
using UtilScripts;

namespace ObjectScripts
{
    public class Character: Substance
    {
        public CharacterProperties CharacterProperties;

        public override void Initialize(Vector2Int globalCoord, int areaIdentity)
        {
            base.Initialize(globalCoord, areaIdentity);
        }

        public bool Move(Vector2Int delta, int moveTime)
        {
            if (delta == Vector2Int.zero) return true;
            var endPos = GlobalPos + delta;
            SpriteController.SetDirection(Utils.VectorToDirection(delta));
            if (Physics2D.OverlapPoint(endPos, BlockLayer) != null)
            {
                return false;
            }

            Collider2D.offset = delta;
            GlobalCoord += delta;
            GlobalPos = endPos;
            StartCoroutine(SmoothMovement(endPos, moveTime));
            return true;
        }

        protected IEnumerator SmoothMovement(Vector3 end, int moveTime)
        {
            var sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            var moveSteps = (int) (moveTime);
            var moveLength = sqrRemainingDistance / moveSteps;
            SpriteController.StartMoving();
            for (; moveSteps != 0; moveSteps--)
            {
                var newPos = Vector3.MoveTowards(transform.position, end, moveLength);
                transform.position = newPos;
                SpriteRenderer.sortingOrder = -Utils.FloatToInt(newPos.y);
                Collider2D.offset = end - newPos;
                yield return null;
            }
            SpriteController.StopMoving();
        }
    }
}