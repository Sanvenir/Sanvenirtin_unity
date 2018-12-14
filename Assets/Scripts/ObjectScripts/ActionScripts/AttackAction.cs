using System;
using ObjectScripts.SpriteController;
using UnityEngine;
using UtilScripts;
using Object = UnityEngine.Object;

namespace ObjectScripts.ActionScripts
{
    public class AttackAction: BaseAction
    {
        public Character Target;
        public string[] AttackPartList = new []
        {
            "Top", "TopLow", "Center"
        };
        
        public AttackAction(Character self) : base(self)
        {
        }

        public override void DoAction(bool check = true)
        {
            CostTime = Self.GetActTime();
            Self.ActivateTime += CostTime;
            check = !check || CheckAction();
            Self.AttackMovement(TargetDirection, CostTime);
            if (!check)
            {
                SceneManager.Instance.Print(Self.name + " attacked empty");
                return;
            }

            var part = AttackPartList[Utils.ProcessRandom.Next(AttackPartList.Length)];
            SceneManager.Instance.Print(
                Self.Name + " attacked " + Target.Name + " on " + Self.BodyParts[part].Name);
            Target.Attacked(Self.GetBaseAttack(), part);
            if (Self.AttackActionEffect == null) return;
            var instance = Object.Instantiate(
                Self.AttackActionEffect, 
                Target.gameObject.transform);
            instance.Initialize();
        }

        public override bool CheckAction()
        {
            Target = Self.MoveCheck<Character>(Utils.DirectionToVector(TargetDirection));
            return Target != null;
        }
    }
}