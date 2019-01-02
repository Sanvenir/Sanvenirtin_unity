using System;
using System.Collections.Generic;
using ObjectScripts.BodyPartScripts;
using UnityEngine;
using UtilScripts;

namespace ObjectScripts.CharSubstance
{
    [Serializable]
    public class Properties
    {
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

        public int Lifetime;
        
        public List<BodyPart> BodyParts = new List<BodyPart>
        {
            {CharacterBodyPart.CreateHead()},
            {CharacterBodyPart.CreateLeftArm()},
            {CharacterBodyPart.CreateRightArm()},
            {CharacterBodyPart.CreateLeftHand()},
            {CharacterBodyPart.CreateRightHand()},
            
        };

//        public Dictionary<string, BodyPart> BodyParts = new Dictionary<string, BodyPart>
//        {
//            {"Head", CharacterBodyPart.CreateHead()},
//            {"LeftArm", CharacterBodyPart.CreateLeftArm()}, 
//            {"RightArm", CharacterBodyPart.CreateRightArm()},
//            {"LeftHand", CharacterBodyPart.CreateLeftHand()},
//            {"RightHand", CharacterBodyPart.CreateRightHand()}
//        };

        // Get Methods
        private int _reactTime;

        public int GetReactTime()
        {
            return _reactTime;
        }

        private int _actRatio;

        public int GetActRatio()
        {
            return _actRatio;
        }

        private int _moveRatio;

        public int GetMoveRatio()
        {
            return _moveRatio;
        }

        public int GetMoveTime()
        {
            return GetMoveRatio() * GetReactTime();
        }

        public int GetActTime()
        {
            return GetActRatio() * GetReactTime();
        }

        public float GetMaxHealth()
        {
            return Constitution * 10;
        }

        public float GetMaxSanity()
        {
            return WillPower * 10;
        }

        public float GetMaxEndure()
        {
            return Constitution * 10;
        }

        public float GetMaxHunger()
        {
            return 200;
        }

        public float GetBaseAttack()
        {
            return Strength;
        }

        public float GetBaseRecover()
        {
            return Metabolism / 10000f;
        }

        public float GetHealthRecover()
        {
            return GetBaseRecover() * 10f;
        }

        public float GetEndureRecover()
        {
            return GetMaxEndure() * GetBaseRecover();
        }

        public void RefreshProperties()
        {
            _reactTime = Mathf.Max(1, (int) (100.0f / Mathf.Log(1f + Speed)));
            _actRatio = Mathf.Max(1, (int) (10 * Mathf.Log(Speed, 10 + ActSpeed)));
            _moveRatio = Mathf.Max(1, (int) (10 * Mathf.Log(Speed, 10 + MoveSpeed)));
        }
    }
}