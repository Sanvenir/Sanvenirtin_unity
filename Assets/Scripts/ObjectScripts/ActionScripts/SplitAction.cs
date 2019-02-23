using ObjectScripts.BodyPartScripts;
using ObjectScripts.CharSubstance;
using UtilScripts.Text;

namespace ObjectScripts.ActionScripts
{
    /// <inheritdoc />
    /// <summary>
    ///     Split a ComplexObject
    /// </summary>
    public class SplitAction : BaseAction
    {
        private readonly BodyPart _bodyPart;
        private readonly ComplexObject _target;

        public SplitAction(Character self, ComplexObject target, BodyPart bodyPart) : base(self)
        {
            _target = target;
            _bodyPart = bodyPart;
            Self.Endure += Self.Properties.Strength.Use(0);
            CostTime = Self.Properties.GetActTime(0.1f);
        }

        public override bool DoAction()
        {
            if (!_bodyPart.Available) return false;
            Self.ActivateTime += CostTime;
            var intensity = _bodyPart.DoDamage(Self.Properties.GetCutAttack(1f));
            Self.Controller.PrintMessage(
                GameText.Instance.GetSplitBodyPartLog(Self.TextName, _target.TextName, _bodyPart.TextName));
            if (intensity < float.Epsilon)
                Self.Controller.PrintMessage(
                    GameText.Instance.GetSplitBodyPartFailedLog(Self.TextName, _target.TextName, _bodyPart.TextName));
            if (_bodyPart.Available) return false;
            Self.Controller.PrintMessage(
                GameText.Instance.GetSplitBodyPartSuccessLog(Self.TextName, _target.TextName, _bodyPart.TextName));
            return true;
        }
    }
}