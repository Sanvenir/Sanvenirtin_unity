using UnityEditor;
using UnityEngine;
using UtilScripts;

namespace ObjectScripts.CharacterController.PlayerOrder
{
    public class RestOrder: BaseOrder
    {
        public RestOrder(string name, Character targetCharacter, Direction targetDirection, Vector2Int targetCoord) : base(name, targetCharacter, targetDirection, targetCoord)
        {
        }

        public override BaseOrder DoOrder()
        {
            Controller.NextAction = Controller.WaitAction;
            return this;
        }
    }
}