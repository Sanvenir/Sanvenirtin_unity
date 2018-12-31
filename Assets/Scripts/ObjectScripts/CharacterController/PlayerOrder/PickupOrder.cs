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
            var colliders = Physics2D.OverlapCircleAll(TargetCoord, 2.0f, SceneManager.Instance.ItemFilter.layerMask);
            if (colliders.Length == 0) return null;
            SceneManager.Instance.ObjectListMenu.StartUp(colliders);
            return null;
        }
    }
}