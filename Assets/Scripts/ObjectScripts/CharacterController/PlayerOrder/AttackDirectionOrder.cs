using ObjectScripts.ActionScripts;
using UtilScripts;
using UtilScripts.Text;

namespace ObjectScripts.CharacterController.PlayerOrder
{
    public class AttackDirectionOrder : BaseOrder
    {
        public override BaseOrder DoOrder()
        {
            base.DoOrder();
            Controller.SetAction(new AttackNeighbourAction(Player, Controller.TargetDirection));
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