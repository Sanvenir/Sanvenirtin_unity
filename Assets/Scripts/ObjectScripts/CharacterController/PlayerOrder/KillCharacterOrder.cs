using UnityEngine;
using UtilScripts;

namespace ObjectScripts.CharacterController.PlayerOrder
{
    public class KillCharacterOrder: BaseOrder
    {
        private Vector2Int _vecInt;

        public override BaseOrder DoOrder()
        {
            if (TargetCharacter == null || TargetCharacter.Dead || TargetCharacter.IsDestroy)
            {
                // If Target Character is dead or destroyed
                Controller.NextAction = null;
                return null;
            }
            Controller.AttackAction.TargetDirection = Utils.VectorToDirection(
                TargetCharacter.WorldCoord - Controller.Character.WorldCoord);
            if (Controller.AttackAction.CheckAction() && Controller.AttackAction.Target == TargetCharacter)
            {
                // If Target Character can be attacked
                Controller.NextAction = Controller.AttackAction;
                return this;
            }
            // Find path to Target Character
            _vecInt = Controller.AStarFinder(TargetCharacter.WorldCoord);
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

        public KillCharacterOrder(string name, Character targetCharacter, Direction targetDirection, Vector2Int targetCoord) : base(name, targetCharacter, targetDirection, targetCoord)
        {
        }
    }
}