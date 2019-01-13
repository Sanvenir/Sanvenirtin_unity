using ObjectScripts.ActionScripts;
using ObjectScripts.CharSubstance;
using UnityEngine;
using UtilScripts;
using UtilScripts.Text;

namespace ObjectScripts.CharacterController.PlayerOrder
{
    public class AttackDirectionOrder: BaseOrder
    {

        public override BaseOrder DoOrder()
        {
            base.DoOrder();
            Controller.SetAction(new AttackNeighbourAction(Controller.Character, Controller.TargetDirection));
            return null;
        }

        public override string GetTextName()
        {
            return GameText.Instance.AttackDirectionOrder;
        }

        public override bool CheckAndSet()
        {
            return Controller.TargetDirection != Direction.None;
        }
    }
}