using System;
using System.Collections.Generic;
using UnityEngine.Analytics;

namespace UtilScripts.Text
{
    [Serializable]
    public class GameText
    {
        public const string SaveDir = @"Resources\GameData\GameText";
        public const string SaveName = "Text-Lang-ZH";
        
        public string AttackDirectionOrder = "攻击";
        public string PickupOrder = "拾起";
        public string RestOrder = "休息";
        public string WalkToOrder = "移至";
        public string ConsumeAct = "食用";
        public string DropAct = "丢弃";

        private static GameText _instance = null;

        public static GameText Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = JsonData.LoadDataFromFile<GameText>(SaveDir, SaveName);
                if (_instance != null) return _instance;
                _instance = new GameText();
                JsonData.SaveDataToFile(SaveDir, SaveName, _instance);
                return _instance;
            }
        }

        public string CannotFindPathLog = "你不知道如何到达这里。";

        public List<string> EatUpItemLog = new List<string>
        {
            "<item>被<self>吃光了",
        };

        public string GetEatUpItemLog(string item, string self)
        {
            return Utils.GetRandomElement(EatUpItemLog)
                .Replace("<self>", self)
                .Replace("<item>", item);
        }

        public List<string> EatItemLog = new List<string>
        {
            "<self>食用了<item>。",
        };

        public string GetEatItemLog(string item, string self)
        {
            return Utils.GetRandomElement(EatItemLog)
                .Replace("<self>", self)
                .Replace("<item>", item);
        }

        public List<string> CharacterDeadLog = new List<string>
        {
            "<self>死掉了。"
        };

        public string GetCharacterDeadLog(string self)
        {
            return Utils.GetRandomElement(CharacterDeadLog)
                .Replace("<self>", self);
        }

        public List<string> SubstanceDestroyLog = new List<string>
        {
            "<self>完全损毁了。"
        };

        public string GetSubstanceDestroyLog(string self)
        {
            return Utils.GetRandomElement(SubstanceDestroyLog)
                .Replace("<self>", self);
        }

        public List<string> BodyPartDestroyLog = new List<string>
        {
            "<self>的<targetPart>完全毁掉了！",
            "<self>失去了<targetPart>！"
        };

        public string GetBodyPartDestroyLog(string self, string targetPart)
        {
            return Utils.GetRandomElement(BodyPartDestroyLog)
                .Replace("<self>", self)
                .Replace("<targetPart>", targetPart);
        }

        public List<string> AttackLog = new List<string>
        {
            "<self>击中了<target>的<targetPart>。",
            "<target>被<self>击中了<targetPart>。",
        };

        public string GetAttackLog(string self, string target, string targetPart)
        {
            return Utils.GetRandomElement(AttackLog)
                .Replace("<self>", self)
                .Replace("<target>", target)
                .Replace("<targetPart>", targetPart);
        }

        public List<string> AttackEmptyLog = new List<string>
        {
            "<self>没有击中任何东西。",
            "<self>的攻击落空了"
        };

        public string GetAttackEmptyLog(string self)
        {
            return Utils.GetRandomElement(AttackEmptyLog)
                .Replace("<self>", self);
        }
    }
}