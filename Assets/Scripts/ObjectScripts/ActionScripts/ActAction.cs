using ObjectScripts.BodyPartScripts;
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
    public abstract class ActAction : BaseAction
    {
        protected readonly ActActionSkill ActionSkill;
        protected readonly Direction TargetDirection;

        protected DamageValue ActDamage;

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
            ActionSkill = actionSkill;
            TargetDirection = targetDirection;
            CostTime = ActionSkill.ActCostTimeRatio * self.Properties.GetActTime(0);
            ActDamage = 
                ActionSkill.BaseDamage +
                ActionSkill.DexterityDamage * Self.Properties.Dexterity.Use(1f) +
                ActionSkill.StrengthDamage * Self.Properties.Strength.Use(1f);
        }

        protected void AttackEmptyLog()
        {
            Self.Controller.PrintMessage(GameText.Instance.GetAttackEmptyLog(Self.TextName));
        }

        public override bool DoAction()
        {
            
            Self.ActivateTime += CostTime;
            Self.Endure += Self.Properties.Strength.Use() * ActionSkill.EndureRatio;
            
            if (!Self.CheckEndure())
            {
                Self.Controller.PrintMessage(GameText.Instance.GetAttackExceedEndureLog(Self.TextName));
                return false;
            }
            Self.AttackMovement(TargetDirection, CostTime);
            return true;
        }
    }

}