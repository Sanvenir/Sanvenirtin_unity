using System.Collections.Generic;
using ObjectScripts.CharSubstance;
using UnityEngine;
using UnityEngine.UI;
using UtilScripts;
using UtilScripts.Text;

namespace ObjectScripts.CharacterController.PlayerOrder
{
    public class PickupOrder: BaseOrder
    {

        private List<BaseObject> _visibleObjects;
        public override BaseOrder DoOrder()
        {
            base.DoOrder();
            SceneManager.Instance.ObjectListMenu.StartUp(_visibleObjects, true);
            return null;
        }

        public override string GetTextName()
        {
            return GameText.Instance.PickupOrder;
        }

        public override bool CheckAndSet()
        {
            _visibleObjects = new List<BaseObject>();
            var colliders = Physics2D.OverlapCircleAll(Controller.Character.WorldPos, 2.0f, SceneManager.Instance.ItemLayer);
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