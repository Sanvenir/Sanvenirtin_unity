using ObjectScripts.CharSubstance;
using UnityEngine;
using UnityEngine.UI;
using UtilScripts;
using UtilScripts.Text;

namespace ObjectScripts.CharacterController.PlayerOrder
{
    public class PickupOrder: BaseOrder
    {

        public override BaseOrder DoOrder()
        {
            base.DoOrder();
            var colliders = Physics2D.OverlapCircleAll(Controller.Character.WorldPos, 2.0f, SceneManager.Instance.ItemFilter.layerMask);
            if (colliders.Length == 0) return null;
            SceneManager.Instance.ObjectListMenu.StartUp(colliders);
            return null;
        }

        public override string GetTextName()
        {
            return GameText.Instance.PickupOrder;
        }
    }
}