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
        [Serializable]
        public struct PotentialProperty
        {
            public float Value;
            public float MaxValue;

            /// <summary>
            ///     Each time the property is used, Value increased by PotentialRate * (MaxValue - Value)
            ///     So PotentialRate always less than 1
            /// </summary>
            public float PotentialRate;

            /// <summary>
            ///     Called after using this property, and the value increased
            /// </summary>
            /// <param name="intense">The increasing intense of this call, always less than 1</param>
            /// <returns></returns>
            public float Use(float intense = 0f)
            {
                Value += (MaxValue - Value) * PotentialRate * intense;
                return Value;
            }

            public PotentialProperty(float value = 10.0f, float maxValue = 20.0f, float potentialRate = 0.0001f)
            {
                Value = value;
                MaxValue = Math.Max(value, maxValue);
                PotentialRate = potentialRate;
            }
        }

        public PotentialProperty Speed = new PotentialProperty(10, 20, 0.0001f);
        public PotentialProperty MoveSpeed = new PotentialProperty(10, 20, 0.0001f);
        public PotentialProperty ActSpeed = new PotentialProperty(10, 20, 0.0001f);
        
        public PotentialProperty Strength = new PotentialProperty(10, 20, 0.0001f);
        public PotentialProperty Dexterity = new PotentialProperty(10, 20, 0.0001f);
        public PotentialProperty Constitution = new PotentialProperty(10, 20, 0.0001f);
        public PotentialProperty Metabolism = new PotentialProperty(10, 20, 0.0001f);

        public PotentialProperty WillPower = new PotentialProperty(10, 20, 0.0001f);
        public PotentialProperty Intelligence = new PotentialProperty(10, 20, 0.0001f);
        public PotentialProperty Perception = new PotentialProperty(10, 20, 0.0001f);

        public PotentialProperty MaxHunger = new PotentialProperty(200, 1000, 0.00001f);

        public int Lifetime;

        // Body parts in properties is belong to the whole race, so do not refactor
        // if only one certain character instance's body parts change.
        public List<BodyPart> BodyParts = new List<BodyPart>
        {
            new BodyPart
            {
                Name = "Head",
                TextName = "头部",
                PartPos = PartPos.High,
                ComponentIndex = 1
            },
            new BodyPart
            {
                Name = "Neck",
                TextName = "颈部",
                PartPos = PartPos.High,
                ComponentIndex = 1
            },
            new BodyPart
            {
                Name = "Chest",
                TextName = "胸部",
                PartPos = PartPos.Middle,
                ComponentIndex = 1
            },
            new BodyPart
            {
                Name = "Waist",
                TextName = "腰部",
                PartPos = PartPos.Middle,
                ComponentIndex = 1
            },
            new BodyPart
            {
                Name = "Crotch",
                TextName = "胯部",
                PartPos = PartPos.Low,
                ComponentIndex = 1
            },
            new BodyPart
            {
                Name = "LeftArm",
                TextName = "左臂",
                PartPos = PartPos.Middle,
                AttachBodyPart = "LeftHand",
                ComponentIndex = 1
            },
            new BodyPart
            {
                Name = "LeftHand",
                TextName = "左手",
                PartPos = PartPos.Middle,
                Fetchable = true,
                ComponentIndex = 1
            },
            new BodyPart
            {
                Name = "RightArm",
                TextName = "右臂",
                PartPos = PartPos.Middle,
                AttachBodyPart = "RightHand",
                ComponentIndex = 1
            },
            new BodyPart
            {
                Name = "RightHand",
                TextName = "右手",
                PartPos = PartPos.Middle,
                Fetchable = true,
                ComponentIndex = 1
            },
            new BodyPart
            {
                Name = "LeftLeg",
                TextName = "左腿",
                PartPos = PartPos.Low,
                ComponentIndex = 1
            },
            new BodyPart
            {
                Name = "LeftFoot",
                TextName = "左足",
                PartPos = PartPos.Low,
                ComponentIndex = 1
            },
            new BodyPart
            {
                Name = "RightLeg",
                TextName = "右腿",
                PartPos = PartPos.Low,
                ComponentIndex = 1
            },
            new BodyPart
            {
                Name = "RightFoot",
                TextName = "右足",
                PartPos = PartPos.Low,
                ComponentIndex = 1
            }
        };

        // Use Methods

        private int _reactTime;

        public int GetReactTime(float intense)
        {
            Speed.Use(intense);
            return _reactTime;
        }

        private int _actRatio;

        public int GetActRatio(float intense)
        {
            ActSpeed.Use(intense);
            return _actRatio;
        }

        private int _moveRatio;

        public int GetMoveRatio(float intense)
        {
            MoveSpeed.Use(intense);
            return _moveRatio;
        }

        public int GetMoveTime(float intense)
        {
            return GetMoveRatio(intense) * GetReactTime(intense);
        }

        public int GetActTime(float intense)
        {
            return GetActRatio(intense) * GetReactTime(intense);
        }

        public float GetMaxHealth(float intense)
        {
            return Constitution.Use(intense) * 10;
        }

        public float GetMaxSanity(float intense)
        {
            return WillPower.Use(intense) * 10;
        }

        public float GetMaxEndure(float intense)
        {
            return Constitution.Use(intense) * 10;
        }

        public float GetMaxHunger(float intense)
        {
            return MaxHunger.Use(intense) * 10;
        }

        public DamageValue GetBaseAttack(float intense)
        {
            return new DamageValue(hitDamage: Strength.Use(intense));
        }

        public float GetBaseRecover(float intense)
        {
            return Metabolism.Use(intense) / 10000f;
        }

        public float GetHealthRecover(float intense)
        {
            return GetBaseRecover(intense) * 10f;
        }

        public float GetEndureRecover(float intense)
        {
            return GetMaxEndure(intense) * GetBaseRecover(intense);
        }

        public void RefreshProperties()
        {
            _reactTime = Mathf.Max(1, (int) (100.0f / Mathf.Log(1f + Speed.Value)));
            _actRatio = Mathf.Max(1, (int) (10 * Mathf.Log(Speed.Value, 10 + ActSpeed.Value)));
            _moveRatio = Mathf.Max(1, (int) (10 * Mathf.Log(Speed.Value, 10 + MoveSpeed.Value)));
        }

        public float GetSensibleRangeSqr(float intense)
        {
            return Perception.Use(intense);
        }

        public float GetVisibleRangeSqr(float intense)
        {
            return Perception.Use(intense) * Perception.Use(intense);
        }

        public float GetVisibleRange(float intense)
        {
            return Perception.Use(intense);
        }

        public float GetWillRecover(float intense)
        {
            return WillPower.Use(intense) * 0.01f;
        }
    }
}