using UnityEngine;
using UtilScripts;

namespace ObjectScripts.CharacterController.PlayerOrder
{
    public class AttackDirectionOrder: BaseOrder
    {
        public AttackDirectionOrder(string name, Character targetCharacter, Direction targetDirection, Vector2Int targetCoord) : base(name, targetCharacter, targetDirection, targetCoord)
        {
        }

        public override BaseOrder DoOrder()
        {
            Controller.AttackAction.TargetDirection = TargetDirection;
            Controller.NextAction = Controller.AttackAction;
            return null;
        }
    }
}