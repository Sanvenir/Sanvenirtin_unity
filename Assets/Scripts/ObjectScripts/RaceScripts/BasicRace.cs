using System;
using System.Collections.Generic;
using ExceptionScripts;
using ObjectScripts.CharSubstance;
using ObjectScripts.SpriteController;
using UnityEditor;
using UnityEngine;
using UtilScripts;

namespace ObjectScripts.RaceScripts
{
    public class BasicRace : MonoBehaviour
    {
        public float AppearRatio;

        public CharacterProperties StandardProperties;

        public List<RandomSprite> SpriteList;

        public virtual void RefactorGameObject()
        {
            if (SpriteList.Count == 0)
            {
                throw new GameException("SpriteList not set");
            }
            
            var randomSprite = SpriteList[Utils.ProcessRandom.Next(SpriteList.Count)];

            var age = Utils.ProcessRandom.Next(randomSprite.MinAge, randomSprite.MaxAge);
            var character = GetComponent<Character>();
            character.Properties = RefactorProperties(age, randomSprite.Gender);
            
            var staticSpriteController = GetComponent<StaticSpriteController>();
            if (staticSpriteController != null)
            {
                if (randomSprite.DisabledSprite != null)
                    staticSpriteController.DisabledSprite = randomSprite.DisabledSprite;
                staticSpriteController.Sprites = randomSprite.Sprites;
                return;
            }
            
            var dynamicSpriteController = GetComponent<DynamicSpriteController>();
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

        private CharacterProperties RefactorProperties(int age, Gender gender)
        {
            var properties = new CharacterProperties
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
                Gender = gender,
                Age = age
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