using System;
using UnityEngine;

namespace ObjectScripts.CharSubstance
{
    [Serializable]
    public class CharacterProperties
    {
        [HideInInspector] public float Health = 0;
        [HideInInspector] public float Sanity = 0;
        [HideInInspector] public float Endure = 0;
        [HideInInspector] public float Hunger = 0;
        [HideInInspector] public float Marady = 0; // Similar to Mana

        public float Speed = 10;
        public float MoveSpeed = 10;
        public float ActSpeed = 10;

        public float Strength = 10;
        public float Dexterity = 10;
        public float Constitution = 10;
        public float Metabolism = 10;

        public float WillPower = 10;
        public float Intelligence = 10;
        public float Perception = 10;


        public Gender Gender;
        public string Race;
        public int Age;
        public int Lifetime;

        // Get Methods
        private int _reactTime;

        public virtual int GetReactTime()
        {
            return _reactTime;
        }

        private int _actRatio;

        public virtual int GetActRatio()
        {
            return _actRatio;
        }

        private int _moveRatio;

        public virtual int GetMoveRatio()
        {
            return _moveRatio;
        }

        public virtual int GetMoveTime()
        {
            return GetMoveRatio() * GetReactTime();
        }

        public virtual int GetActTime()
        {
            return GetActRatio() * GetReactTime();
        }

        public virtual float GetMaxHealth()
        {
            return Constitution * 10;
        }

        public virtual float GetMaxSanity()
        {
            return WillPower * 10;
        }

        public virtual float GetMaxEndure()
        {
            return Constitution * 10;
        }

        public virtual float GetMaxHunger()
        {
            return 200;
        }

        public virtual float GetBaseAttack()
        {
            return Strength;
        }

        public virtual float GetBaseRecover()
        {
            return Metabolism / 10000f;
        }

        public virtual float GetHealthRecover()
        {
            return Mathf.Min((GetBaseRecover() * 10f), Health);
        }

        public virtual float GetEndureRecover()
        {
            return Mathf.Min((GetMaxEndure() * GetBaseRecover()), Endure);
        }

        public void RefreshProperties()
        {
            _reactTime = Mathf.Max(1, (int) (100.0f / Mathf.Log(1f + Speed)));
            _actRatio = Mathf.Max(1, (int) (10 * Mathf.Log(Speed, 10 + ActSpeed)));
            _moveRatio = Mathf.Max(1, (int) (10 * Mathf.Log(Speed, 10 + MoveSpeed)));
        }
    }
}