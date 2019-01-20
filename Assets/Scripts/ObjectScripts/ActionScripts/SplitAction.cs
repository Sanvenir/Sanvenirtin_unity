using ObjectScripts.BodyPartScripts;
using ObjectScripts.CharSubstance;
using UtilScripts.Text;

namespace ObjectScripts.ActionScripts
{
    /// <inheritdoc />
    /// <summary>
    /// Split a ComplexObject
    /// </summary>
    public class SplitAction: BaseAction
    {
        private readonly ComplexObject _target;
        private readonly BodyPart _bodyPart;
        public SplitAction(Character self, ComplexObject target, BodyPart bodyPart) : base(self)
        {
            _target = target;
            _bodyPart = bodyPart;
            Self.Endure += Self.Properties.Strength.Use(0);
            CostTime = Self.Properties.GetActTime(0.1f);
        }

        public override bool DoAction()
        {
            base.DoAction();
            if (!_bodyPart.Available) return false;
            var intensity = _bodyPart.DoDamage(Self.Properties.GetCutAttack(1f));
            SceneManager.Instance.Print(
                GameText.Instance.GetSplitBodyPartLog(Self.TextName, _target.TextName, _bodyPart.TextName), 
                Self.WorldCoord);
            if (intensity < float.Epsilon)
            {
                SceneManager.Instance.Print(
                    GameText.Instance.GetSplitBodyPartFailedLog(Self.TextName, _target.TextName, _bodyPart.TextName), 
                    Self.WorldCoord);
            }
            if (_bodyPart.Available) return false;
            SceneManager.Instance.Print(
                GameText.Instance.GetSplitBodyPartSuccessLog(Self.TextName, _target.TextName, _bodyPart.TextName), 
                Self.WorldCoord);
            return true;
        }
    }
}