using System;
using System.Collections.Generic;
using ExceptionScripts;
using ObjectScripts.CharSubstance;
using ObjectScripts.SpriteController;
using UnityEditor;
using UnityEngine;
using UtilScripts;
using UtilScripts.Text;
using BodyPart = ObjectScripts.BodyPartScripts.BodyPart;

namespace ObjectScripts.RaceScripts
{
    [Serializable]
    public class BasicRace
    {
        public string Name = "human";
        public string TextName = "人类";
        public const string SaveDir =  @"Resources\GameData\RaceProperties";
        public string NameFormat = "general";
        public List<RandomSprite> SpriteList;
        
        private RandomName _randomName;
        public RandomName RandomName
        {
            get { return _randomName ?? (_randomName = RandomName.GetInstance(NameFormat)); }
        }

//        public Properties StandardProperties = new Properties();

        public static BasicRace LoadFromFile(string name)
        {
            var result = JsonData.LoadDataFromFile<BasicRace>(SaveDir, name);
            if (result != null) return result;
            result = new BasicRace {Name = name};
            JsonData.SaveDataToFile(SaveDir, name, result);
            return result;
        }

        public void SaveToFile()
        {
            JsonData.SaveDataToFile(SaveDir, Name, this);
        }

        private Properties _standardProperties;
        public Properties StandardProperties
        {
            get
            {
                if (_standardProperties != null)
                    return _standardProperties;

                _standardProperties = JsonData.LoadDataFromFile<Properties>(SaveDir, Name);
                if (_standardProperties != null)
                    return _standardProperties;

                _standardProperties = new Properties();
                JsonData.SaveDataToFile(SaveDir, Name, _standardProperties);
                return _standardProperties;
            }
        }

        public void RandomRefactor(Character character)
        {
            if (SpriteList.Count == 0)
            {
                throw new GameException("SpriteList not set");
            }

            var randomSprite = SpriteList[Utils.ProcessRandom.Next(SpriteList.Count)];
            
            character.Age = Utils.ProcessRandom.Next(randomSprite.MinAge, randomSprite.MaxAge);
            character.Gender = randomSprite.Gender;
            character.TextName = RandomName.GenerateName(character.Gender);

            RefactorGameObject(character);

            var staticSpriteController = character.GetComponent<StaticSpriteController>();
            if (staticSpriteController != null)
            {
                if (randomSprite.DisabledSprite != null)
                    staticSpriteController.DisabledSprite = randomSprite.DisabledSprite;
                staticSpriteController.Sprites = randomSprite.Sprites;
                return;
            }

            var dynamicSpriteController = character.GetComponent<DynamicSpriteController>();
            if (dynamicSpriteController == null) return;
            if (randomSprite.DisabledSprite != null)
                dynamicSpriteController.DisabledSprite = randomSprite.DisabledSprite;
            dynamicSpriteController.MoveDownSprites = new List<Sprite>()
            {
                {randomSprite.Sprites[1]},
                {randomSprite.Sprites[0]},
                {randomSprite.Sprites[1]},
                {randomSprite.Sprites[2]},
            };
            dynamicSpriteController.MoveLeftSprites = new List<Sprite>()
            {
                {randomSprite.Sprites[4]},
                {randomSprite.Sprites[3]},
                {randomSprite.Sprites[4]},
                {randomSprite.Sprites[5]},
            };
            dynamicSpriteController.MoveRightSprites = new List<Sprite>()
            {
                {randomSprite.Sprites[7]},
                {randomSprite.Sprites[6]},
                {randomSprite.Sprites[7]},
                {randomSprite.Sprites[8]},
            };
            dynamicSpriteController.MoveUpSprites = new List<Sprite>()
            {
                {randomSprite.Sprites[10]},
                {randomSprite.Sprites[9]},
                {randomSprite.Sprites[10]},
                {randomSprite.Sprites[11]},
            };
            dynamicSpriteController.MoveNoneSprites = new List<Sprite>()
            {
                {randomSprite.Sprites[1]}
            };


            dynamicSpriteController.StopDownSprites = new List<Sprite>()
            {
                {randomSprite.Sprites[1]}
            };
            dynamicSpriteController.StopLeftSprites = new List<Sprite>()
            {
                {randomSprite.Sprites[4]}
            };
            dynamicSpriteController.StopRightSprites = new List<Sprite>()
            {
                {randomSprite.Sprites[7]}
            };
            dynamicSpriteController.StopUpSprites = new List<Sprite>()
            {
                {randomSprite.Sprites[10]}
            };
            dynamicSpriteController.StopNoneSprites = new List<Sprite>()
            {
                {randomSprite.Sprites[1]}
            };
        }

        public void RefactorGameObject(Character character)
        {
            character.BodyParts = new Dictionary<string, BodyPart>();
            character.Properties = CreateProperties(character.Age, character.Gender);
            foreach (var bodyPart in StandardProperties.BodyParts)
            {
                character.BodyParts.Add(bodyPart.Name, (BodyPart) bodyPart.Clone());
            }
        }

        private Properties CreateProperties(int age, Gender gender)
        {
            var properties = new Properties
            {
                Speed = StandardProperties.Speed,
                MoveSpeed = StandardProperties.MoveSpeed,
                ActSpeed = StandardProperties.ActSpeed,
                Strength = StandardProperties.Strength,
                Dexterity = StandardProperties.Dexterity,
                Constitution = StandardProperties.Constitution,
                Metabolism = StandardProperties.Metabolism,
                WillPower = StandardProperties.WillPower,
                Intelligence = StandardProperties.Intelligence,
                Perception = StandardProperties.Perception,
                BodyParts = StandardProperties.BodyParts, 
                Lifetime = StandardProperties.Lifetime
            };
            properties.RefreshProperties();
            return properties;
        }

        [Serializable]
        public struct RandomSprite
        {
            public Sprite[] Sprites;
            public Sprite DisabledSprite;

            public int MaxAge;
            public int MinAge;

            public Gender Gender;
        }
    }
}