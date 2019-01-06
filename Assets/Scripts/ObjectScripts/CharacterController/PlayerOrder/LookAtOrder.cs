using UnityEngine;
using UtilScripts;
using UtilScripts.Text;

namespace ObjectScripts.CharacterController.PlayerOrder
{
    public class LookAtOrder: BaseOrder
    {
        public override BaseOrder DoOrder()
        {
            base.DoOrder();
            var colliders = Physics2D.OverlapCircleAll(
                SceneManager.Instance.WorldCoordToPos(Controller.TargetCoord),
                2.0f, SceneManager.Instance.ItemFilter.layerMask);
            if (colliders.Length == 0) return null;
            SceneManager.Instance.ObjectListMenu.StartUp(colliders, false);
            return null;
        }

        public override string GetTextName()
        {
            return GameText.Instance.LookAtOrder;
        }

        public override bool IsAvailable()
        {
            var colliders = Physics2D.OverlapCircleAll(
                SceneManager.Instance.WorldCoordToPos(Controller.TargetCoord),
                2.0f, SceneManager.Instance.ItemFilter.layerMask);
            return colliders.Length != 0 && Controller.TargetDirection != Direction.None;
        }
    }
}