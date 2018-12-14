using UtilScripts;

namespace ObjectScripts.BodyPartScripts
{
    public static class HumanBodyPart
    {
        public static BodyPart CreateHead()
        {
            return new BodyPart
            {
                Name = "Head",
                Essential = true, 
                Size = 3,
                Defence = 10,
                Durability = new LimitValue(1000)
            };
        }

        public static BodyPart CreateNeck()
        {
            return new BodyPart()
            {
                Name = "Neck",
                Essential = true,
                Size = 1,
                Defence = 0,
                Durability = new LimitValue(200)
            };
        }

        public static BodyPart CreateChest()
        {
            return new BodyPart()
            {
                Name = "Chest",
                Essential = true,
                Size = 10,
                Defence = 10,
                Durability = new LimitValue(2000)
            };
        }
        

        public static BodyPart CreateWaist()
        {
            return new BodyPart()
            {
                Name = "Waist",
                Essential = true,
                Size = 5,
                Defence = 5,
                Durability = new LimitValue(1000)
            };
        }
        
        
        public static BodyPart CreateCrotch()
        {
            return new BodyPart()
            {
                Name = "Crotch",
                Essential = true,
                Size = 10,
                Defence = 5,
                Durability = new LimitValue(2000)
            };
        }
        

        public static BodyPart CreateLeftArm()
        {
            return new BodyPart()
            {
                Name = "LeftArm",
                Essential = false,
                Size = 3,
                Defence = 10,
                Durability = new LimitValue(500)
            };
        }

        public static BodyPart CreateLeftHand()
        {
            return new BodyPart()
            {
                Name = "LeftHand",
                Essential = false,
                Size = 1,
                Defence = 10,
                Durability = new LimitValue(200)
            };
        }

        public static BodyPart CreateRightArm()
        {
            return new BodyPart()
            {
                Name = "RightArm",
                Essential = false,
                Size = 3,
                Defence = 10,
                Durability = new LimitValue(500)
            };
        }

        public static BodyPart CreateRightHand()
        {
            return new BodyPart()
            {
                Name = "RightHand",
                Essential = false,
                Size = 1,
                Defence = 10,
                Durability = new LimitValue(200)
            };
        }
        
        public static BodyPart CreateLeftLeg()
        {
            return new BodyPart()
            {
                Name = "LeftLeg",
                Essential = false,
                Size = 3,
                Defence = 10,
                Durability = new LimitValue(500)
            };
        }

        public static BodyPart CreateLeftFoot()
        {
            return new BodyPart()
            {
                Name = "LeftFoot",
                Essential = false,
                Size = 1,
                Defence = 10,
                Durability = new LimitValue(200)
            };
        }

        public static BodyPart CreateRightLeg()
        {
            return new BodyPart()
            {
                Name = "RightLeg",
                Essential = false,
                Size = 3,
                Defence = 10,
                Durability = new LimitValue(500)
            };
        }

        public static BodyPart CreateRightFoot()
        {
            return new BodyPart()
            {
                Name = "RightFoot",
                Essential = false,
                Size = 1,
                Defence = 10,
                Durability = new LimitValue(200)
            };
        }
    }
}