using ObjectScripts.CharSubstance;
using UnityEngine;
using UtilScripts;

namespace ObjectScripts.CharacterController.PlayerOrder
{
    public abstract class BaseOrder: INamed
    {
        public static PlayerController Controller;
        public Character TargetCharacter;
        public Direction TargetDirection;
        public Vector2Int TargetCoord;

        public string Name;
        public BaseOrder[] ChildOrders;

        public virtual BaseOrder DoOrder()
        {
            // rtype: The next order for player controller, null means no order or invalid order
            return null;
        }

        public BaseOrder(
            Character targetCharacter, 
            Direction targetDirection, 
            Vector2Int targetCoord)
        {
            TargetCharacter = targetCharacter;
            TargetDirection = targetDirection;
            TargetCoord = targetCoord;
        }

        public abstract string GetTextName();
    }
}