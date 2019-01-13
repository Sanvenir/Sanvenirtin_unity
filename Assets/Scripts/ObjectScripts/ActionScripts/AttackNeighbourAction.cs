using ObjectScripts.BodyPartScripts;
using ObjectScripts.CharSubstance;
using UnityEngine;
using UtilScripts;
using UtilScripts.Text;

namespace ObjectScripts.ActionScripts
{
    /// <inheritdoc />
    /// <summary>
    ///     An action make character attack a certain direction
    /// </summary>
    public class AttackNeighbourAction : BaseAction
    {
        private readonly PartPos _attackPartPos;
        private readonly Direction _targetDirection;

        private Character _target;

        public AttackNeighbourAction(Character self, Direction targetDirection,
            PartPos attackPartPos = PartPos.Arbitrary)
            : base(self)
        {
            _targetDirection = targetDirection;
            _attackPartPos = attackPartPos;
            CostTime = Self.Properties.GetActTime();
        }

        private void AttackEmpty()
        {
            SceneManager.Instance.Print(
                GameText.Instance.GetAttackEmptyLog(Self.TextName), Self.WorldCoord);
        }

        /// <inheritdoc />
        /// <summary>
        ///     Character attacks the AttackPartPos of character on TargetDirection, if there is no character or character has no
        ///     part on the AttackPartPos then return false; Attack also affect the attitude of target character.
        /// </summary>
        /// <returns></returns>
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

            // Affect target AIController
            _target.Controller.GetHostility(Self, 10);

            var attackBodyParts = _target.GetBodyParts(_attackPartPos);
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
                    _target.GetBodyParts(_attackPartPos)[part].TextName
                ), Self.WorldCoord);
            _target.Attacked(
                Self.Properties.GetBaseAttack(),
                _target.GetBodyParts(_attackPartPos)[part]);

            // Play the attack action effect animation
            _target.PlayEffect(Self.AttackActionEffect);
            return true;
        }
    }
}