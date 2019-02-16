using System;
using System.Collections.Generic;
using UtilScripts;

namespace ObjectScripts.ItemScripts
{
    [Serializable]
    public class WeaponSetting
    {
        [Serializable]
        public struct WeaponType
        {
            public string Name;
            public List<int> MainTypeList;
        }

        public const string SaveDir = @"Resources\GameData\GameSetting";
        public const string SaveName = "WeaponSetting";


        private static WeaponSetting _instance;

        public static WeaponSetting Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = JsonData.LoadDataFromFile<WeaponSetting>(SaveDir, SaveName);
                if (_instance != null) return _instance;
                _instance = new WeaponSetting();
                JsonData.SaveDataToFile(SaveDir, SaveName, _instance);
                return _instance;
            }
        }

        public List<string> WeaponMainType = new List<string>
        {
            /*0*/"手类",
            /*1*/"双刃剑类",
            /*2*/"单刃剑类",
            /*3*/"矛类",
            /*4*/"斧类",
            /*5*/"锤类",
            /*6*/"棍类",
            /*7*/"盾类",
            /*8*/"鞭类",
            /*9*/"奇形类",
            /*10*/"小型武器",
            /*11*/"中型武器",
            /*12*/"大型武器"
        };

        /// <summary>
        ///     ID: -1 means the fetched object is not a weapon
        ///     0 means (or equally means) no fetched object, i.e. empty-handed
        /// </summary>
        public List<WeaponType> WeaponTypeList = new List<WeaponType>
        {
            new WeaponType
            {
                Name = "空手",
                MainTypeList = new List<int> {0}
            },
            new WeaponType
            {
                Name = "长剑",
                MainTypeList = new List<int> {1, 11}
            }
        };
    }
}