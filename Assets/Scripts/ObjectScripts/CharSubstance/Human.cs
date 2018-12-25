using ObjectScripts.BodyPartScripts;
using UnityEngine;

namespace ObjectScripts.CharSubstance
{
    public class Human : Character
    {
        // BodyParts
        public BodyPart Head = CharacterBodyPart.CreateHead();
        public BodyPart Neck = CharacterBodyPart.CreateNeck();
        public BodyPart Chest = CharacterBodyPart.CreateChest();
        public BodyPart Waist = CharacterBodyPart.CreateWaist();
        public BodyPart Crotch = CharacterBodyPart.CreateCrotch();
        public BodyPart LeftArm = CharacterBodyPart.CreateLeftArm();
        public BodyPart LeftHand = CharacterBodyPart.CreateLeftHand();
        public BodyPart RightArm = CharacterBodyPart.CreateRightArm();
        public BodyPart RightHand = CharacterBodyPart.CreateRightHand();
        public BodyPart LeftLeg = CharacterBodyPart.CreateLeftLeg();
        public BodyPart LeftFoot = CharacterBodyPart.CreateLeftFoot();
        public BodyPart RightLeg = CharacterBodyPart.CreateRightLeg();
        public BodyPart RightFoot = CharacterBodyPart.CreateRightFoot();

        public override void RefreshProperties()
        {
            base.RefreshProperties();
        }
        
        public override void Initialize(Vector2Int worldCoord, int areaIdentity)
        {

            HighParts.Add(Head);
            HighParts.Add(Neck);
            HighParts.Add(Chest);

            MiddleParts.Add(Waist);
            MiddleParts.Add(LeftArm);
            MiddleParts.Add(RightArm);
            MiddleParts.Add(LeftHand);
            MiddleParts.Add(RightHand);
            
            LowParts.Add(Crotch);
            LowParts.Add(LeftFoot);
            LowParts.Add(LeftLeg);
            LowParts.Add(RightFoot);
            LowParts.Add(RightLeg);

            LeftArm.AttachBodyPart = LeftHand;
            RightArm.AttachBodyPart = RightHand;
            LeftLeg.AttachBodyPart = LeftFoot;
            RightLeg.AttachBodyPart = RightFoot;

            Neck.AttachBodyPart = Head;
            
            foreach (var part in GetAllBodyParts())
            {
                BodyParts.Add(part.Name, part);
            }

            FetchDictionary.Add(LeftHand, null);
            FetchDictionary.Add(RightHand, null);
            
            base.Initialize(worldCoord, areaIdentity);
        }
    }
}