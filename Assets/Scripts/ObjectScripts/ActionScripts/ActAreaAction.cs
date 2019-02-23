using ObjectScripts.CharSubstance;
using ObjectScripts.StyleScripts.ActStyleScripts;
using UnityEngine;
using UtilScripts;
using UtilScripts.Text;

namespace ObjectScripts.ActionScripts
{
    public class ActAreaAction : ActAction
    {
        /// <inheritdoc />
        /// <summary>
        ///     Initializer
        /// </summary>
        /// <param name="self">Self character</param>
        /// <param name="actionSkill">result of these action, can set the parameter and function of this action</param>
        /// <param name="targetDirection"></param>
        public ActAreaAction(Character self, ActActionSkill actionSkill, Direction targetDirection) : base(self,
            actionSkill, targetDirection)
        {
        }

        private static LayerMask AttackLayer
        {
            get { return SceneManager.Instance.AttackLayer; }
        }

        public override bool DoAction()
        {
            if (!base.DoAction()) return false;
            var flag = false;

            foreach (var attackPlace in ActionSkill.GetAttackPlaces(TargetDirection, Self))
            {
                var target = BaseObject.GetObject<Substance>(attackPlace, AttackLayer);
                if (target == null) continue;
                flag = true;

                if (ActionSkill.IsAllParts)
                {
                    foreach (var bodyPart in target.GetBodyParts(ActionSkill.TargetPartPos))
                    {
                        target.Attacked(ActDamage, bodyPart);
                        Self.Controller.PrintMessage(GameText.Instance.GetAttackLog(Self.TextName, target.TextName,
                            bodyPart.TextName, ActionSkill.GetTextName()));
                    }
                }
                else
                {
                    var sum = 0f;
                    foreach (var bodyPart in target.GetBodyParts(ActionSkill.TargetPartPos)) sum += bodyPart.Size;

                    sum *= (float) Utils.ProcessRandom.NextDouble();
                    foreach (var bodyPart in target.GetBodyParts(ActionSkill.TargetPartPos))
                    {
                        sum -= bodyPart.Size;
                        if (!(sum <= 0)) continue;
                        target.Attacked(ActDamage, bodyPart);
                        Self.Controller.PrintMessage(GameText.Instance.GetAttackLog(Self.TextName, target.TextName,
                            bodyPart.TextName, ActionSkill.GetTextName()));
                    }

                    break;
                }

                // TODO: Implement substance and character attacked effect
            }

            // TODO: Add Effect Animations

            if (flag) return true;
            AttackEmptyLog();
            return false;
        }
    }
}