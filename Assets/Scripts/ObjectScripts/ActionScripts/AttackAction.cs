using System;
using ObjectScripts.SpriteController;
using UnityEngine;
using UtilScripts;
using Object = UnityEngine.Object;

namespace ObjectScripts.ActionScripts
{
    [Serializable]
    public class AttackAction: DirectionAction
    {
        public Character Target;
        
        public AttackAction(Character self) : base(self)
        {
        }

        public override void DoAction(bool check = true)
        {
            CostTime = Self.GetActTime();
            Self.AttackMovement(TargetDirection, CostTime);
            Self.ActivateTime += CostTime;
            if (check)
            {
                if (!CheckAction())
                {
                    SceneManager.Instance.Print(Self.name + " attacked empty");
                    return;
                }
            }
            Target.Attacked(Self.GetBaseAttack(), "Chest");
            SceneManager.Instance.Print(Self.name + " attacked " + Target.name);
            if (Self.AttackActionEffect == null) return;
            var instance = Object.Instantiate(
                Self.AttackActionEffect, 
                Target.gameObject.transform);
            instance.Initialize();
        }

        public override bool CheckAction()
        {
            Self.TurnTo(TargetDirection);
            Target = Self.MoveCheck<Character>(Utils.DirectionToVector(TargetDirection));
            return Target != null;
        }
    }
}