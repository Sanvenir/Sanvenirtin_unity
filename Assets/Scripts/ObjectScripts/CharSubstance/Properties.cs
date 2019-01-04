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
            new BodyPart
            {
                Name = "Head",
                Essential = true,
                PartPos = PartPos.High
            },
            new BodyPart
            {
                Name = "Neck",
                Essential = true,
                PartPos = PartPos.High
            },
            new BodyPart
            {
                Name = "Chest",
                Essential = true,
                PartPos = PartPos.Middle
            },
            new BodyPart
            {
            Name = "Waist",
            Essential = false,
            PartPos = PartPos.Middle
            },
            new BodyPart
            {
                Name = "Crotch",
                Essential = false,
                PartPos = PartPos.Low
            },
            new BodyPart
            {
                Name = "LeftArm",
                Essential = false,
                PartPos = PartPos.Middle, 
                AttachBodyPart = "LeftHand"
            },
            new BodyPart
            {
                Name = "LeftHand",
                Essential = false,
                PartPos = PartPos.Middle, 
                Fetchable = true
            },
            new BodyPart
            {
                Name = "RightArm",
                Essential = false,
                PartPos = PartPos.Middle, 
                AttachBodyPart = "RightHand"
            },
            new BodyPart
            {
                Name = "RightHand",
                Essential = false,
                PartPos = PartPos.Middle,
                Fetchable = true
            },
            new BodyPart
            {
                Name = "LeftLeg",
                Essential = false,
                PartPos = PartPos.Low
            },
            new BodyPart
            {
                Name = "LeftFoot",
                Essential = false,
                PartPos = PartPos.Low
            },
            new BodyPart
            {
                Name = "RightLeg",
                Essential = false,
                PartPos = PartPos.Low
            },
            new BodyPart
            {
                Name = "RightFoot",
                Essential = false,
                PartPos = PartPos.Low
            }
        };

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