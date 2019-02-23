using ObjectScripts.ActionScripts;
using ObjectScripts.StyleScripts.ActStyleScripts;

namespace ObjectScripts.CharacterController.PlayerOrder
{
    public class BasicActOrder : BaseOrder
    {
        private readonly ActActionSkill _actionSkill;

        public BasicActOrder(ActActionSkill actionSkill)
        {
            _actionSkill = actionSkill;
        }

        public override string GetTextName()
        {
            return _actionSkill.GetTextName();
        }

        public override bool CheckAndSet()
        {
            return true;
        }

        public override BaseOrder DoOrder()
        {
            if (_actionSkill.IsStab)
                SceneManager.Instance.PartSelectPanel.StartUp(Controller.TargetDirection, _actionSkill);
            else
                Controller.SetAction(new ActAreaAction(Player, _actionSkill, Controller.TargetDirection));
            return null;
        }
    }
}