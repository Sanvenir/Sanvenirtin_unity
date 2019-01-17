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
        private ComplexObject _target;
        public SplitAction(Character self, ComplexObject target) : base(self)
        {
            _target = target;
            Self.Endure += Self.Properties.Strength.Use(0);
            CostTime = Self.Properties.GetActTime(0.1f);
        }

        public override bool DoAction()
        {
            base.DoAction();
            foreach (var bodyPart in _target.BodyParts.Values)
            {
                if (!bodyPart.Available) continue;
                var intensity = bodyPart.DoDamage(Self.Properties.GetCutAttack(1f));
                SceneManager.Instance.Print(
                    GameText.Instance.GetSplitBodyPartLog(Self.TextName, _target.TextName, bodyPart.TextName), 
                    Self.WorldCoord);
                if (intensity < float.Epsilon)
                {
                    SceneManager.Instance.Print(
                        GameText.Instance.GetSplitBodyPartFailedLog(Self.TextName, _target.TextName, bodyPart.TextName), 
                        Self.WorldCoord);
                }
                if (bodyPart.Available) continue;
                SceneManager.Instance.Print(
                    GameText.Instance.GetSplitBodyPartSuccessLog(Self.TextName, _target.TextName, bodyPart.TextName), 
                    Self.WorldCoord);
            }

            return true;
        }
    }
}