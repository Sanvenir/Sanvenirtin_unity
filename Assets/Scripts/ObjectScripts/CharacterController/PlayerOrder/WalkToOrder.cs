using ObjectScripts.ActionScripts;
using UnityEngine;
using UtilScripts;
using UtilScripts.Text;

namespace ObjectScripts.CharacterController.PlayerOrder
{
    public class WalkToOrder : BaseOrder
    {
        private Vector2Int _vecInt;

        public override BaseOrder DoOrder()
        {
            base.DoOrder();
            if (Player.WorldCoord == Controller.TargetCoord) return null;

            _vecInt = Controller.AStarFinder(
                Controller.TargetCoord,
                (int) Player.Properties.Intelligence.Use(0.1f));
            if (_vecInt == Vector2Int.zero || !Player.MoveCheck(_vecInt))
            {
                SceneManager.Instance.Print(
                    GameText.Instance.CannotFindPathLog, Player.WorldCoord);
                return null;
            }

            Controller.SetAction(new MoveAction(Player, Utils.VectorToDirection(_vecInt)));
            return this;
        }

        public override string GetTextName()
        {
            return GameText.Instance.WalkToOrder;
        }

        public override bool CheckAndSet()
        {
            return Controller.TargetDirection != Direction.None;
        }
    }
}