using System;
using ObjectScripts.ComponentScripts;

namespace ObjectScripts
{
    [Serializable]
    public class HumanProperties
    {
        public Component Head = new Component();
        public Component Neck = new Component();
        public Component Shoulder = new Component();
        public Component Chest = new Component();
        public Component Waist = new Component();
        public Component LeftArm = new Component();
        public Component LeftHand = new Component();
        public Component RightArm = new Component();
        public Component RightHand = new Component();
        public Component Crotch = new Component();
        public Component LeftLeg = new Component();
        public Component LeftFoot = new Component();
        public Component RightLeg = new Component();
        public Component RightFoot = new Component();
        public Component Tail = new Component();

        public void RefreshProperties()
        {
        }

        public HumanProperties()
        {
            LeftArm.AttachComponent = LeftHand;
            RightArm.AttachComponent = RightHand;
            LeftLeg.AttachComponent = LeftFoot;
            RightLeg.AttachComponent = RightFoot;
        }

        private int _moveDexterity;
        private int _moveStrength;
        private int _attackStrength;
        private int _attackDexterity;

        public int GetMoveDexterity()
        {
            return (LeftLeg.UseDexterity() +
                    RightLeg.UseDexterity() +
                    LeftFoot.UseDexterity() +
                    RightFoot.UseDexterity()) / 4;
        }

        public int GetMoveStrength()
        {
            return (LeftLeg.UseStrength() +
                    RightLeg.UseStrength()) / 2;
        }

        public int GetAttackStrength()
        {
            return (LeftArm.UseStrength() +
                    RightArm.UseStrength()) / 2;
        }

        public int GetAttackDexterity()
        {
            return (LeftArm.UseDexterity() + 
                    RightArm.UseDexterity()) / 2;
        }
    }
}