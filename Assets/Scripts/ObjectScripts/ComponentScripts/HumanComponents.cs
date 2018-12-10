using UtilScripts;

namespace ObjectScripts.ComponentScripts
{
    public static class HumanComponents
    {
        public static SubstanceComponent CreateHead()
        {
            return new SubstanceComponent
            {
                Name = "Head",
                Essential = true, 
                Size = 3,
                Defence = 10,
                Durability = new LimitValue(1000)
            };
        }

        public static SubstanceComponent CreateNeck()
        {
            return new SubstanceComponent()
            {
                Name = "Neck",
                Essential = true,
                Size = 1,
                Defence = 0,
                Durability = new LimitValue(200)
            };
        }

        public static SubstanceComponent CreateChest()
        {
            return new SubstanceComponent()
            {
                Name = "Chest",
                Essential = true,
                Size = 10,
                Defence = 10,
                Durability = new LimitValue(2000)
            };
        }
        

        public static SubstanceComponent CreateWaist()
        {
            return new SubstanceComponent()
            {
                Name = "Waist",
                Essential = true,
                Size = 5,
                Defence = 5,
                Durability = new LimitValue(1000)
            };
        }
        
        
        public static SubstanceComponent CreateCrotch()
        {
            return new SubstanceComponent()
            {
                Name = "Crotch",
                Essential = true,
                Size = 10,
                Defence = 5,
                Durability = new LimitValue(2000)
            };
        }
        

        public static SubstanceComponent CreateLeftArm()
        {
            return new SubstanceComponent()
            {
                Name = "LeftArm",
                Essential = false,
                Size = 3,
                Defence = 10,
                Durability = new LimitValue(500)
            };
        }

        public static SubstanceComponent CreateLeftHand()
        {
            return new SubstanceComponent()
            {
                Name = "LeftHand",
                Essential = false,
                Size = 1,
                Defence = 10,
                Durability = new LimitValue(200)
            };
        }

        public static SubstanceComponent CreateRightArm()
        {
            return new SubstanceComponent()
            {
                Name = "RightArm",
                Essential = false,
                Size = 3,
                Defence = 10,
                Durability = new LimitValue(500)
            };
        }

        public static SubstanceComponent CreateRightHand()
        {
            return new SubstanceComponent()
            {
                Name = "RightHand",
                Essential = false,
                Size = 1,
                Defence = 10,
                Durability = new LimitValue(200)
            };
        }
        
        public static SubstanceComponent CreateLeftLeg()
        {
            return new SubstanceComponent()
            {
                Name = "LeftLeg",
                Essential = false,
                Size = 3,
                Defence = 10,
                Durability = new LimitValue(500)
            };
        }

        public static SubstanceComponent CreateLeftFoot()
        {
            return new SubstanceComponent()
            {
                Name = "LeftFoot",
                Essential = false,
                Size = 1,
                Defence = 10,
                Durability = new LimitValue(200)
            };
        }

        public static SubstanceComponent CreateRightLeg()
        {
            return new SubstanceComponent()
            {
                Name = "RightLeg",
                Essential = false,
                Size = 3,
                Defence = 10,
                Durability = new LimitValue(500)
            };
        }

        public static SubstanceComponent CreateRightFoot()
        {
            return new SubstanceComponent()
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