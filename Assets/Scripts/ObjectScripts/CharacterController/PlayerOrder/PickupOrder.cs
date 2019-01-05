using ObjectScripts.CharSubstance;
using UnityEngine;
using UnityEngine.UI;
using UtilScripts;
using UtilScripts.Text;

namespace ObjectScripts.CharacterController.PlayerOrder
{
    public class PickupOrder: BaseOrder
    {
        
        public PickupOrder(Character targetCharacter, Direction targetDirection, Vector2Int targetCoord) : base(targetCharacter, targetDirection, targetCoord)
        {
            Name = "PickUp";
        }

        public override BaseOrder DoOrder()
        {
            var colliders = Physics2D.OverlapCircleAll(TargetCoord, 2.0f, SceneManager.Instance.ItemFilter.layerMask);
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