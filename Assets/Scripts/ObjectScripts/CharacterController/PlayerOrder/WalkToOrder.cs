using ObjectScripts.ActionScripts;
using ObjectScripts.CharSubstance;
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
            if (Controller.Character.WorldCoord == Controller.TargetCoord)
            {
                return null;
            }

            _vecInt = Controller.AStarFinder(
                Controller.TargetCoord,
                (int) Controller.Character.Properties.Intelligence);
            if (_vecInt == Vector2Int.zero || !Controller.Character.MoveCheck(_vecInt))
            {
                SceneManager.Instance.Print(
                    GameText.Instance.CannotFindPathLog);
                return null;
            }

            Controller.SetAction(new WalkAction(Controller.Character, Utils.VectorToDirection(_vecInt)));
            return this;
        }

        public override string GetTextName()
        {
            return GameText.Instance.WalkToOrder;
        }
    }
}