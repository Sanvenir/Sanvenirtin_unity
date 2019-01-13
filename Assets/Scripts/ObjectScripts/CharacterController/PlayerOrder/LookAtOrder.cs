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
        public override BaseOrder DoOrder()
        {
            base.DoOrder();
            SceneManager.Instance.ObjectListMenu.StartUp(_visibleObjects, false);
            return null;
        }

        public override string GetTextName()
        {
            return GameText.Instance.LookAtOrder;
        }

        public override bool CheckAndSet()
        {
            _visibleObjects = new List<BaseObject>();
            var colliders = Physics2D.OverlapCircleAll(
                SceneManager.Instance.WorldCoordToPos(Controller.TargetCoord),
                2.0f, SceneManager.Instance.PlayerLookAtLayer);
            foreach (var collider in colliders)
            {
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