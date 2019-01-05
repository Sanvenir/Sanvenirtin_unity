using ObjectScripts.CharSubstance;
using UnityEngine;
using UtilScripts;
using UtilScripts.Text;

namespace ObjectScripts.CharacterController.PlayerOrder
{
    public class AttackDirectionOrder: BaseOrder
    {
        public AttackDirectionOrder(Character targetCharacter, Direction targetDirection, Vector2Int targetCoord) : 
            base(targetCharacter, targetDirection, targetCoord)
        {
            Name = "Attack";
        }

        public override BaseOrder DoOrder()
        {
            Controller.AttackAction.TargetDirection = TargetDirection;
            Controller.NextAction = Controller.AttackAction;
            return null;
        }

        public override string GetTextName()
        {
            return GameText.Instance.AttackDirectionOrder;
        }
    }
}