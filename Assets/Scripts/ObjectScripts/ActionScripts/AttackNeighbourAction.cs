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
    public class AttackNeighbourAction: BaseAction
    {
        private readonly PartPos _attackPart;
        private readonly Direction _targetDirection;
        
        private Character _target;
        
        public AttackNeighbourAction(Character self, Direction targetDirection, PartPos attackPart = PartPos.Arbitrary) : base(self)
        {
            _targetDirection = targetDirection;
            _attackPart = attackPart;
            CostTime = Self.Properties.GetActTime();
        }

        private void AttackEmpty()
        {
            SceneManager.Instance.Print(
                GameText.Instance.GetAttackEmptyLog(Self.TextName));
        }

        public override bool DoAction()
        {
            base.DoAction();
            Self.Endure += Self.Properties.Strength;
            Self.MoveCheck(Utils.DirectionToVector(_targetDirection), out _target);
            Self.AttackMovement(_targetDirection, CostTime);

            if (_target == null)
            {
                AttackEmpty();
                return false;
            }
            
            var attackBodyParts = _target.GetBodyParts(_attackPart);
            if (attackBodyParts.Count == 0)
            {
                AttackEmpty();
                return false;
            }

            var part = Utils.ProcessRandom.Next(attackBodyParts.Count);
            SceneManager.Instance.Print(
                GameText.Instance.GetAttackLog(
                    Self.TextName, 
                    _target.TextName, 
                    _target.GetBodyParts(_attackPart)[part].TextName
                    ));
            _target.Attacked(
                Self.Properties.GetBaseAttack(), 
                _target.GetBodyParts(_attackPart)[part]);
            
            // Affect target AIController
            _target.Controller.GetHostility(Self, 10);

            if (Self.AttackActionEffect == null) return true;
            var instance = Object.Instantiate(
                Self.AttackActionEffect, 
                _target.gameObject.transform);
            instance.Initialize();
            return true;
        }
    }
}