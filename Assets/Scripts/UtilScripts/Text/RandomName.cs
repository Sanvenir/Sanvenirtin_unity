using System;
using System.Collections.Generic;
using System.Linq;
using ObjectScripts.CharSubstance;
using UIScripts;
using UnityEngine;

namespace UtilScripts.Text
{
    [Serializable]
    public class RandomName
    {
        public const string SaveDir = "Resources/GameData/RandomName";

        public List<string> NeutralSyllable = new List<string>
        {
            "Neutral Syllable"
        };
        public List<string> PositiveSyllable = new List<string>
        {
            "Positive Syllable"
        };
        public List<string> NegativeSyllable = new List<string>
        {
            "Negative Syllable"
        };

        private static readonly Dictionary<string, RandomName> Instances = new Dictionary<string, RandomName>();

        public static RandomName GetInstance(string name)
        {
            if (Instances.ContainsKey(name))
            {
                return Instances[name];
            }

            Instances.Add(name, LoadFromFile(name));
            return Instances[name];
        }


        public List<string> GetSyllable(Gender gender)
        {
            switch (gender)
            {
                case Gender.None:
                    return NegativeSyllable;
                case Gender.Male:
                    return PositiveSyllable;
                case Gender.Female:
                    return NegativeSyllable;
                default:
                    return PositiveSyllable;
            }
        }
        
        public string GenerateName(Gender gender)
        {
            if (GetSyllable(gender).Count == 0)
            {
                gender = Gender.None;
            }

            var index = Utils.ProcessRandom.Next(GetSyllable(gender).Count);
            return GetSyllable(gender)[index];
        }

        private static RandomName LoadFromFile(string name)
        {
            var result = JsonData.LoadDataFromFile<RandomName>(SaveDir, name);
            if (result != null) return result;
            result = new RandomName();
            JsonData.SaveDataToFile(SaveDir, name, result);
            return result;
        }
    }
}