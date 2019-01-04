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

        public List<string> NeutralSyllable = new List<string>();
        public List<string> PositiveSyllable = new List<string>();
        public List<string> NegativeSyllable = new List<string>();


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

        public void SaveToFile(string name)
        {
            JsonData.SaveDataToFile(SaveDir, name, this);
        }

        public static RandomName LoadFromFile(string name)
        {
            return JsonData.LoadDataFromFile<RandomName>(SaveDir, name);
        }
    }
}