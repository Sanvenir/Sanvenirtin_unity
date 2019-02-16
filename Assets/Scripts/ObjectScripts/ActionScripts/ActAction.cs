using ObjectScripts.CharSubstance;
using ObjectScripts.StyleScripts.ActStyleScripts;
using UnityEngine;
using UtilScripts;
using UtilScripts.Text;

namespace ObjectScripts.ActionScripts
{
    /// <inheritdoc />
    /// <summary>
    /// Action with set result
    /// </summary>
    public class ActAction: BaseAction
    {
        private readonly Direction _targetDirection;
        private readonly ActActionSkill _actionSkill;

        private static LayerMask AttackLayer
        {
            get { return SceneManager.Instance.AttackLayer; }
        }

        /// <summary>
        /// Initializer
        /// </summary>
        /// <param name="self">Self character</param>
        /// <param name="actionSkill">result of these action, can set the parameter and function of this action</param>
        /// <param name="targetDirection"></param>
        public ActAction(Character self, ActActionSkill actionSkill, Direction targetDirection) : base(self)
        {
            _actionSkill = actionSkill;
            _targetDirection = targetDirection;
            CostTime = _actionSkill.ActCostTimeRatio * self.Properties.GetActTime(0);
        }
        
        private void AttackEmpty()
        {
            Self.Controller.PrintMessage(GameText.Instance.GetAttackEmptyLog(Self.TextName));
        }

        public override bool DoAction()
        {
            Self.ActivateTime += CostTime;
            Self.Endure += Self.Properties.Strength.Use() * _actionSkill.EndureRatio;
            
            if (!Self.CheckEndure())
            {
                Self.Controller.PrintMessage(GameText.Instance.GetAttackExceedEndureLog(Self.TextName));
                return false;
            }

            Self.AttackMovement(_targetDirection, CostTime);

            var flag = false;

            foreach (var attackPlace in _actionSkill.GetAttackPlaces(_targetDirection, Self))
            {
                var hit = Physics2D.OverlapPoint(attackPlace, AttackLayer);
                if (hit == null) continue;
                var substance = hit.GetComponent<Substance>();
                if (substance == null) continue;
                flag = true;
                var damage =
                    _actionSkill.BaseDamage +
                    _actionSkill.DexterityDamage * Self.Properties.Dexterity.Use(1f) +
                    _actionSkill.StrengthDamage * Self.Properties.Strength.Use(1f);

                if (_actionSkill.IsAllParts)
                {
                }

                // TODO: Implement substance and character attacked effect
            }
            
            // TODO: Add Effect Animations

            if (flag) return true;
            AttackEmpty();
            return false;
        }
    }
}