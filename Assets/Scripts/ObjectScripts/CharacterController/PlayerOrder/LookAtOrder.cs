using System.Collections.Generic;
using UnityEngine;
using UtilScripts.Text;

namespace ObjectScripts.CharacterController.PlayerOrder
{   
    /// <inheritdoc />
    /// <summary>
    ///     Show a round range of items around the given coord
    /// </summary>
    public class LookAtOrder : BaseOrder
    {
        private List<BaseObject> _visibleObjects;

        private static Collider2D CursorCollider
        {
            get { return SceneManager.Instance.SceneControlButton.CursorCollider; }
        }
        public override BaseOrder DoOrder()
        {
            base.DoOrder();
            SceneManager.Instance.GroundObjectListMenu.StartUp(_visibleObjects, false);
            return null;
        }

        public override string GetTextName()
        {
            return GameText.Instance.LookAtOrder;
        }

        public override bool CheckAndSet()
        {
            if (CursorCollider == null) return false;
            _visibleObjects = new List<BaseObject>();
            var colliders = new Collider2D[20];
            CursorCollider.OverlapCollider(SceneManager.Instance.PlayerLookAtFilter, colliders);
            foreach (var collider in colliders)
            {
                if (collider == null) break;
                var baseObject = collider.GetComponent<BaseObject>();
                if (baseObject != null && baseObject.Visible)
                {
                    _visibleObjects.Add(baseObject);
                }
            }
            return _visibleObjects.Count != 0;
        }
    }
}