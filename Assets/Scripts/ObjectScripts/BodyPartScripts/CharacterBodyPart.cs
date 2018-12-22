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
                Essential = true
            };
        }

        public static BodyPart CreateNeck()
        {
            return new BodyPart
            {
                Name = "Neck",
                Essential = true
            };
        }

        public static BodyPart CreateChest()
        {
            return new BodyPart
            {
                Name = "Chest",
                Essential = true
            };
        }
        

        public static BodyPart CreateWaist()
        {
            return new BodyPart
            {
                Name = "Waist",
                Essential = false
            };
        }
        
        
        public static BodyPart CreateCrotch()
        {
            return new BodyPart
            {
                Name = "Crotch",
                Essential = false
            };
        }
        

        public static BodyPart CreateLeftArm()
        {
            return new BodyPart
            {
                Name = "LeftArm",
                Essential = false
            };
        }

        public static BodyPart CreateLeftHand()
        {
            return new BodyPart
            {
                Name = "LeftHand",
                Essential = false
            };
        }

        public static BodyPart CreateRightArm()
        {
            return new BodyPart
            {
                Name = "RightArm",
                Essential = false
            };
        }

        public static BodyPart CreateRightHand()
        {
            return new BodyPart
            {
                Name = "RightHand",
                Essential = false
            };
        }
        
        public static BodyPart CreateLeftLeg()
        {
            return new BodyPart
            {
                Name = "LeftLeg",
                Essential = false
            };
        }

        public static BodyPart CreateLeftFoot()
        {
            return new BodyPart
            {
                Name = "LeftFoot",
                Essential = false
            };
        }

        public static BodyPart CreateRightLeg()
        {
            return new BodyPart
            {
                Name = "RightLeg",
                Essential = false
            };
        }

        public static BodyPart CreateRightFoot()
        {
            return new BodyPart
            {
                Name = "RightFoot",
                Essential = false
            };
        }

        public static BodyPart CreateLeftWing()
        {
            return new BodyPart
            {
                Name = "LeftWing",
                Essential = false
            };
        }

        public static BodyPart CreateRightWing()
        {
            return new BodyPart
            {
                Name = "RightWing",
                Essential = false
            };
        }

        public static BodyPart CreateTail()
        {
            return new BodyPart
            {
                Name = "Tail",
                Essential = false
            };
        }
    }
}