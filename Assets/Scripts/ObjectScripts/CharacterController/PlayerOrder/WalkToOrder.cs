using UnityEngine;
using UtilScripts;

namespace ObjectScripts.CharacterController.PlayerOrder
{
    public class WalkToOrder: BaseOrder
    {
        private Vector2Int _vecInt;
        public WalkToOrder(string name, Character targetCharacter, Direction targetDirection, Vector2Int targetCoord) : base(name, targetCharacter, targetDirection, targetCoord)
        {
        }

        public override BaseOrder DoOrder()
        {
            if (Controller.Character.WorldCoord == TargetCoord)
            {
                Controller.NextAction = null;
                return null;
            }

            _vecInt = Controller.AStarFinder(TargetCoord);
            if (_vecInt == Vector2Int.zero)
            {
                Controller.NextAction = null;
                SceneManager.Instance.Print(
                    "You can't find path to your order," +
                    " because of your current intelligence");
                return null;
            }

            Controller.WalkAction.TargetDirection = Utils.VectorToDirection(_vecInt);
            Controller.NextAction = Controller.WalkAction;
            return this;
        }
    }
}