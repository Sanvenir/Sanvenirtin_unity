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
        public string[] AttackComponentList = new []
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

            var component = AttackComponentList[Utils.ProcessRandom.Next(AttackComponentList.Length)];
            SceneManager.Instance.Print(
                Self.Name + " attacked " + Target.Name + " on " + Self.Components[component].Name);
            Target.Attacked(Self.GetBaseAttack(), component);
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