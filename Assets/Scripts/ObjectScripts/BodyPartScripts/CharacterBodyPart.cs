using ObjectScripts.CharSubstance;
using UtilScripts;

namespace ObjectScripts.BodyPartScripts
{
    public static class CharacterBodyPart
    {
        public static BodyPart CreateHead()
        {
            
            return new BodyPart
            {
                Name = "Head",
                Essential = true,
                PartPos = PartPos.High
            };
        }

        public static BodyPart CreateNeck()
        {
            return new BodyPart
            {
                Name = "Neck",
                Essential = true,
                PartPos = PartPos.High
            };
        }

        public static BodyPart CreateChest()
        {
            return new BodyPart
            {
                Name = "Chest",
                Essential = true,
                PartPos = PartPos.Middle
            };
        }
        

        public static BodyPart CreateWaist()
        {
            return new BodyPart
            {
                Name = "Waist",
                Essential = false,
                PartPos = PartPos.Middle
            };
        }
        
        
        public static BodyPart CreateCrotch()
        {
            return new BodyPart
            {
                Name = "Crotch",
                Essential = false,
                PartPos = PartPos.Low
            };
        }
        

        public static BodyPart CreateLeftArm()
        {
            return new BodyPart
            {
                Name = "LeftArm",
                Essential = false,
                PartPos = PartPos.Middle, 
                AttachBodyPart = "LeftHand"
            };
        }

        public static BodyPart CreateLeftHand()
        {
            return new BodyPart
            {
                Name = "LeftHand",
                Essential = false,
                PartPos = PartPos.Middle
            };
        }

        public static BodyPart CreateRightArm()
        {
            return new BodyPart
            {
                Name = "RightArm",
                Essential = false,
                PartPos = PartPos.Middle, 
                AttachBodyPart = "RightHand"
            };
        }

        public static BodyPart CreateRightHand()
        {
            return new BodyPart
            {
                Name = "RightHand",
                Essential = false,
                PartPos = PartPos.Middle
            };
        }
        
        public static BodyPart CreateLeftLeg()
        {
            return new BodyPart
            {
                Name = "LeftLeg",
                Essential = false,
                PartPos = PartPos.Low
            };
        }

        public static BodyPart CreateLeftFoot()
        {
            return new BodyPart
            {
                Name = "LeftFoot",
                Essential = false,
                PartPos = PartPos.Low
            };
        }

        public static BodyPart CreateRightLeg()
        {
            return new BodyPart
            {
                Name = "RightLeg",
                Essential = false,
                PartPos = PartPos.Low
            };
        }

        public static BodyPart CreateRightFoot()
        {
            return new BodyPart
            {
                Name = "RightFoot",
                Essential = false,
                PartPos = PartPos.Low
            };
        }

        public static BodyPart CreateLeftWing()
        {
            return new BodyPart
            {
                Name = "LeftWing",
                Essential = false,
                PartPos = PartPos.Low
            };
        }

        public static BodyPart CreateRightWing()
        {
            return new BodyPart
            {
                Name = "RightWing",
                Essential = false,
                PartPos = PartPos.Middle
            };
        }

        public static BodyPart CreateTail()
        {
            return new BodyPart
            {
                Name = "Tail",
                Essential = false,
                PartPos = PartPos.Middle
            };
        }
    }
}