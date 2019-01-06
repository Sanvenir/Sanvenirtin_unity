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
        
        // Body parts in properties is belong to the whole race, so do not refactor
        // if only one certain character instance's body parts change.
        public List<BodyPart> BodyParts = new List<BodyPart>
        {
            new BodyPart
            {
                Name = "Head",
                TextName = "头部",
                Essential = true,
                PartPos = PartPos.High
            },
            new BodyPart
            {
                Name = "Neck",
                TextName = "颈部",
                Essential = true,
                PartPos = PartPos.High
            },
            new BodyPart
            {
                Name = "Chest",
                TextName = "胸部",
                Essential = true,
                PartPos = PartPos.Middle
            },
            new BodyPart
            {
            Name = "Waist",
                TextName = "腰部",
            Essential = false,
            PartPos = PartPos.Middle
            },
            new BodyPart
            {
                Name = "Crotch",
                TextName = "胯部",
                Essential = false,
                PartPos = PartPos.Low
            },
            new BodyPart
            {
                Name = "LeftArm",
                TextName = "左臂",
                Essential = false,
                PartPos = PartPos.Middle, 
                AttachBodyPart = "LeftHand"
            },
            new BodyPart
            {
                Name = "LeftHand",
                TextName = "左手",
                Essential = false,
                PartPos = PartPos.Middle, 
                Fetchable = true
            },
            new BodyPart
            {
                Name = "RightArm",
                TextName = "右臂",
                Essential = false,
                PartPos = PartPos.Middle, 
                AttachBodyPart = "RightHand"
            },
            new BodyPart
            {
                Name = "RightHand",
                TextName = "右手",
                Essential = false,
                PartPos = PartPos.Middle,
                Fetchable = true
            },
            new BodyPart
            {
                Name = "LeftLeg",
                TextName = "左腿",
                Essential = false,
                PartPos = PartPos.Low
            },
            new BodyPart
            {
                Name = "LeftFoot",
                TextName = "左足",
                Essential = false,
                PartPos = PartPos.Low
            },
            new BodyPart
            {
                Name = "RightLeg",
                TextName = "右腿",
                Essential = false,
                PartPos = PartPos.Low
            },
            new BodyPart
            {
                Name = "RightFoot",
                TextName = "右足",
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

        public DamageValue GetBaseAttack()
        {
            return new DamageValue(hitDamage: Strength);
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