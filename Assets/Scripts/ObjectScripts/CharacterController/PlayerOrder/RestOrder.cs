using ObjectScripts.ActionScripts;
using ObjectScripts.CharSubstance;
using UnityEditor;
using UnityEngine;
using UtilScripts;
using UtilScripts.Text;

namespace ObjectScripts.CharacterController.PlayerOrder
{
    public class RestOrder: BaseOrder
    {
        public override BaseOrder DoOrder()
        {
            base.DoOrder();
            Controller.SetAction(new WaitAction(Controller.Character));
            return this;
        }

        public override string GetTextName()
        {
            return GameText.Instance.RestOrder;
        }

        public override bool CheckAndSet()
        {
            return true;
        }
    }
}