using ObjectScripts.BodyPartScripts;
using ObjectScripts.CharSubstance;
using ObjectScripts.StyleScripts.ActStyleScripts;
using UtilScripts;
using UtilScripts.Text;

namespace ObjectScripts.ActionScripts
{
    public class ActStabAction: ActAction
    {
        private BodyPart _targetPart;
        private Substance _target;
        

        public override bool DoAction()
        {
            if (!base.DoAction()) return false;
            if (_targetPart == null)
            {
                // TODO: Add stab arbitrarily effect 
            }
            else
            {
                _target.Attacked(ActDamage, _targetPart);
                Self.Controller.PrintMessage(GameText.Instance.GetAttackLog(Self.TextName, _target.TextName,
                    _targetPart.TextName, ActionSkill.GetTextName()));
            }

            return true;
        }

        public ActStabAction(Character self, ActActionSkill actionSkill, Direction targetDirection, BodyPart targetPart, Substance target) : base(self, actionSkill, targetDirection)
        {
            _targetPart = targetPart;
            _target = target;
        }
    }
}