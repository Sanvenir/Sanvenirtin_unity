using System;
using ObjectScripts.BodyPartScripts;
using ObjectScripts.CharSubstance;
using ObjectScripts.SpriteController;
using UnityEngine;
using UtilScripts;
using UtilScripts.Text;
using Object = UnityEngine.Object;

namespace ObjectScripts.ActionScripts
{
    public class AttackAction: BaseAction
    {
        public Character Target;

        public PartPos AttackPart;
        
        public AttackAction(Character self) : base(self)
        {
            AttackPart = PartPos.Arbitrary;
        }

        public void AttackEmpty()
        {
            SceneManager.Instance.Print(
                GameText.Instance.GetAttackEmptyLog(Self.TextName));
        }

        public override void DoAction(bool check = true)
        {
            CostTime = Self.Properties.GetActTime();
            Self.ActivateTime += CostTime;
            Self.Endure += Self.Properties.GetBaseAttack();
            check = !check || CheckAction();
            Self.AttackMovement(TargetDirection, CostTime);

            if (!check)
            {
                AttackEmpty();
                return;
            }
            
            var attackBodyParts = Target.GetBodyParts(AttackPart);
            if (attackBodyParts.Count == 0)
            {
                AttackEmpty();
                return;
            }

            var part = Utils.ProcessRandom.Next(attackBodyParts.Count);
            SceneManager.Instance.Print(
                GameText.Instance.GetAttackLog(
                    Self.TextName, 
                    Target.TextName, 
                    Target.GetBodyParts(AttackPart)[part].TextName
                    ));
            Target.Attacked(Self.Properties.GetBaseAttack(), Target.GetBodyParts(AttackPart)[part]);
            
            // Affect target AIController
            Target.Controller.GetHostility(Self, 10);
            
            if (Self.AttackActionEffect == null) return;
            var instance = Object.Instantiate(
                Self.AttackActionEffect, 
                Target.gameObject.transform);
            instance.Initialize();
        }

        public override bool CheckAction()
        {
            // If There is no character to attack, return true
            Self.MoveCheck(Utils.DirectionToVector(TargetDirection), out Target);
            return Target != null;
        }
    }
}