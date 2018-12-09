using UtilScripts;

namespace ObjectScripts.ComponentScripts
{
    public static class HumanComponents
    {
        public static SubstanceComponent createHead()
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

        public static SubstanceComponent createNeck()
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

        public static SubstanceComponent createChest()
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
        

        public static SubstanceComponent createWaist()
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
        
        
        public static SubstanceComponent createCrotch()
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
        

        public static SubstanceComponent createLeftArm()
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

        public static SubstanceComponent createLeftHand()
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

        public static SubstanceComponent createRightArm()
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

        public static SubstanceComponent createRightHand()
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
        
        public static SubstanceComponent createLeftLeg()
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

        public static SubstanceComponent createLeftFoot()
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

        public static SubstanceComponent createRightLeg()
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

        public static SubstanceComponent createRightFoot()
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