using System;
using ObjectScripts.BodyPartScripts;
using ObjectScripts.SpriteController;
using UnityEngine;
using UtilScripts;
using Object = UnityEngine.Object;

namespace ObjectScripts.ActionScripts
{
    public class AttackAction: BaseAction
    {
        public Character Target;

        public PartPos AttackPart = PartPos.High;
        
        public AttackAction(Character self) : base(self)
        {
        }

        public override void DoAction(bool check = true)
        {
            CostTime = Self.GetActTime();
            Self.ActivateTime += CostTime;
            Self.Endure += Self.GetBaseAttack();
            check = !check || CheckAction();
            Self.AttackMovement(TargetDirection, CostTime);
            if (!check)
            {
                SceneManager.Instance.Print(Self.name + " attacked empty");
                return;
            }

            var part = Utils.ProcessRandom.Next(Target.GetBodyParts(AttackPart).Count);
            SceneManager.Instance.Print(
                Self.Name + " attacked " + Target.Name + " on " + 
                Target.GetBodyParts(AttackPart)[part].Name);
            Target.Attacked(Self.GetBaseAttack(), Target.GetBodyParts(AttackPart)[part]);
            
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