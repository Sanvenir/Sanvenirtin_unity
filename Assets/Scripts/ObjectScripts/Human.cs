using System;

namespace ObjectScripts
{
    public class Human: Character
    {
        public HumanProperties HumanProperties;

        public override void RefreshProperties()
        {
            base.RefreshProperties();
            _attackTime = 1 + CharacterProperties.ReactionSpeed / HumanProperties.GetAttackDexterity();
            _moveTime =  1 + CharacterProperties.ReactionSpeed / HumanProperties.GetMoveDexterity();
        }

        private int _attackTime;
        public override int GetAttackTime()
        {
            return _attackTime;
        }

        private int _moveTime;
        public override int GetMoveTime()
        {
            return _moveTime;
        }
    }
}