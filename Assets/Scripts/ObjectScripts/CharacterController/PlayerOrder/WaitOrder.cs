using ObjectScripts.ActionScripts;
using UtilScripts;
using UtilScripts.Text;

namespace ObjectScripts.CharacterController.PlayerOrder
{
    public class WaitOrder : BaseOrder
    {
        public override BaseOrder DoOrder()
        {
            Controller.SetAction(new WaitAction(Controller.Character));
            return null;
        }
        
        public override string GetTextName()
        {
            return GameText.Instance.WaitOrder;
        }

        public override bool CheckAndSet()
        {
            return Controller.TargetDirection == Direction.None;
        }
    }
}