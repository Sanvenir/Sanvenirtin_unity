using ObjectScripts.CharSubstance;
using UnityEngine;
using UtilScripts;

namespace ObjectScripts.CharacterController.PlayerOrder
{
    public class PickupOrder: BaseOrder
    {
        public PickupOrder(string name, Character targetCharacter, Direction targetDirection, Vector2Int targetCoord) : base(name, targetCharacter, targetDirection, targetCoord)
        {
        }

        public override BaseOrder DoOrder()
        {
            SceneManager.Instance.ObjectListMenu.enabled = false;
            SceneManager.Instance.ObjectListMenu.ColliderList =
                Physics2D.OverlapCircleAll(TargetCoord, 2.0f, SceneManager.Instance.ItemFilter.layerMask);
            SceneManager.Instance.ObjectListMenu.gameObject.SetActive(true);
            SceneManager.Instance.ObjectListMenu.enabled = true;
            
            return base.DoOrder();
        }
    }
}