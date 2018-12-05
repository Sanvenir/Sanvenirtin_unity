using System;
using UtilScripts;

namespace ObjectScripts
{
    [Serializable]
    public class Component
    {
        public enum ComponentType
        {
            Head, 
            Neck, 
            Shoulder, 
            Chest, 
            Waist, 
            LeftArm, 
            RightArm, 
            LeftHand, 
            RightHand,
            Crotch,
            LeftLeg, 
            RightLeg, 
            LeftFoot, 
            RightFoot, 
            Tail,
            Cylinder,
            Cube, 
            Plane, 
            Ball
        }

        public ComponentType Type;
        public LimitValue Durability = new LimitValue(100);
        public int Size = 10;
        public int Strength = 10;
        public int Dexterity = 10;
        public int Defence = 10;
    }
}