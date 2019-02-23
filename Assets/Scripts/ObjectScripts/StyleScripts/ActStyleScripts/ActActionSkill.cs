using System;
using System.Collections.Generic;
using ObjectScripts.BodyPartScripts;
using ObjectScripts.CharSubstance;
using ObjectScripts.SpriteController;
using UnityEngine;
using UtilScripts;

namespace ObjectScripts.StyleScripts.ActStyleScripts
{
    [Serializable]
    public class ActActionSkill : INamed
    {
        public enum AttackType
        {
            ///One tile forward
            Normal,

            ///Two tiles forward
            Depth,

            ///Three tiles from left to right when at up direction
            FormLeft,

            ///Three tiles form right to left when at up direction
            FormRight,

            ///Four tiles around
            Neighbour,

            ///Eight tiles around
            Around
        }

        private readonly Dictionary<AttackType, IterateCoords> _attackTypeList =
            new Dictionary<AttackType, IterateCoords>
            {
                {AttackType.Normal, IterateNormalAttack},
                {AttackType.Depth, IterateDepthAttack},
                {AttackType.FormLeft, IterateFromLeftAttack},
                {AttackType.FormRight, IterateFromRightAttack},
                {AttackType.Neighbour, IterateNeighbour},
                {AttackType.Around, IterateAround}
            };

        /// <summary>
        ///     The time cost ratio; CostTime = ActTime * ActCostTimeRatio
        /// </summary>
        public int ActCostTimeRatio = 1;

        /// <summary>
        ///     The effect played in given area;
        /// </summary>
        public EffectController AreaEffect;

        public bool AttackAllPart;

        /// <summary>
        ///     The base damage of this act skill
        /// </summary>
        public DamageValue BaseDamage;

        /// <summary>
        ///     Which layer can terminate this attack
        /// </summary>
        public LayerMask BlockLayer;

        /// <summary>
        ///     The Damage Addition by dexterity; Damage = Dexterity * DexterityDamage
        /// </summary>
        public DamageValue DexterityDamage;

        public int EndStyle;

        /// <summary>
        ///     The cost of Endure; EndureCost = Strength * EndureRatio
        /// </summary>
        public float EndureRatio;

        /// <summary>
        ///     Whether all body parts in given TargetPartPos and are attacked
        /// </summary>
        public bool IsAllParts;

        public bool IsStab;

        /// <summary>
        ///     The effect played in each coords;
        /// </summary>
        public EffectController SingleEffect;

        /// <summary>
        ///     The Damage Addition by strength; Damage = Strength * StrengthDamage
        /// </summary>
        public DamageValue StrengthDamage;

        public PartPos TargetPartPos;

        public string TextName;

        public AttackType Type;

        public string GetTextName()
        {
            return TextName;
        }


        public IEnumerable<Vector2> GetAttackPlaces(Direction direction, Character character)
        {
            if (direction == Direction.None || !_attackTypeList.ContainsKey(Type)) yield break;
            foreach (var coord in _attackTypeList[Type](direction))
            {
                var targetPos = character.WorldPos + coord;
                yield return targetPos;

                var block = Physics2D.OverlapPoint(targetPos, BlockLayer);
                if (block != null) break;
            }
        }

        private static IEnumerable<Vector2Int> IterateAround(Direction direction)
        {
            for (var i = -1; i < 2; i++)
            for (var j = -1; j < 2; j++)
            {
                if (i == 0 && j == 0) continue;
                yield return new Vector2Int(i, j);
            }
        }

        private static IEnumerable<Vector2Int> IterateNeighbour(Direction direction)
        {
            yield return Vector2Int.up;
            yield return Vector2Int.right;
            yield return Vector2Int.down;
            yield return Vector2Int.left;
        }


        private static IEnumerable<Vector2Int> IterateFromRightAttack(Direction direction)
        {
            var forward = Utils.DirectionToVector(direction);
            var right = new Vector2Int(forward.y, forward.x);
            yield return forward + right;
            yield return forward;
            yield return forward - right;
        }

        private static IEnumerable<Vector2Int> IterateFromLeftAttack(Direction direction)
        {
            var forward = Utils.DirectionToVector(direction);
            var right = new Vector2Int(forward.y, forward.x);
            yield return forward - right;
            yield return forward;
            yield return forward + right;
        }

        private static IEnumerable<Vector2Int> IterateDepthAttack(Direction direction)
        {
            yield return Utils.DirectionToVector(direction);
            yield return Utils.DirectionToVector(direction) * 2;
        }


        private static IEnumerable<Vector2Int> IterateNormalAttack(Direction direction)
        {
            yield return Utils.DirectionToVector(direction);
        }

        [Serializable]
        public struct TargetPart
        {
            public string PartName;
            public float Accuracy;
        }

        private delegate IEnumerable<Vector2Int> IterateCoords(Direction direction);
    }
}