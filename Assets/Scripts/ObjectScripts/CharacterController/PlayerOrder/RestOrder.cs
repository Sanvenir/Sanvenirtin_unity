using ObjectScripts.CharSubstance;
using UnityEditor;
using UnityEngine;
using UtilScripts;
using UtilScripts.Text;

namespace ObjectScripts.CharacterController.PlayerOrder
{
    public class RestOrder: BaseOrder
    {
        public RestOrder(Character targetCharacter, Direction targetDirection, Vector2Int targetCoord) : base(targetCharacter, targetDirection, targetCoord)
        {
            Name = "Rest";
        }

        public override BaseOrder DoOrder()
        {
            Controller.NextAction = Controller.WaitAction;
            return this;
        }

        public override string GetTextName()
        {
            return GameText.Instance.RestOrder;
        }
    }
}