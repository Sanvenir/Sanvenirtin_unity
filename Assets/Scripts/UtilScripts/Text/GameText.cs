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
        public string WaitOrder = "等待";
        public string LookAtOrder = "查看";
        public string ConsumeAct = "食用";
        public string SplitAct = "拆解";
        public string DropAct = "丢弃";
        public string WalkMoveStyle = "步行中";
        public string RunMoveStyle = "奔跑中";
        public string DashMoveStyle = "冲刺中";
        
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

        public List<string> PartInfoLog = new List<string>
        {
            "\n来自<self>的<part>。",
        };

        public string GetPartInfoLog(string self, string part)
        {
            return Utils.GetRandomElement(PartInfoLog)
                .Replace("<self>", self)
                .Replace("<part>", part);
        }

        public List<string> SplitBodyPartSuccessLog = new List<string>
        {
            "<self>切下了<target>的<targetPart>。",
        };

        public string GetSplitBodyPartSuccessLog(string self, string target, string targetPart)
        {
            return Utils.GetRandomElement(SplitBodyPartSuccessLog)
                .Replace("<self>", self)
                .Replace("<target>", target)
                .Replace("<targetPart>", targetPart);
        }

        public List<string> SplitBodyPartFailedLog = new List<string>
        {
            "<target>的<targetPart>对于<self>来说并不容易切下。",
        };

        public string GetSplitBodyPartFailedLog(string self, string target, string targetPart)
        {
            return Utils.GetRandomElement(SplitBodyPartFailedLog)
                .Replace("<self>", self)
                .Replace("<target>", target)
                .Replace("<targetPart>", targetPart);
        }

        public List<string> SplitBodyPartLog = new List<string>
        {
            "<self>正在尝试切下<target>的<targetPart>。",
            "<self>在分割<target>的<targetPart>。",
        };

        public string GetSplitBodyPartLog(string self, string target, string targetPart)
        {
            return Utils.GetRandomElement(SplitBodyPartLog)
                .Replace("<self>", self)
                .Replace("<target>", target)
                .Replace("<targetPart>", targetPart);
        }

        public List<string> AttackExceedEndureLog = new List<string>
        {
            "<self>因为极度疲劳，无力进行攻击。",
        };

        public string GetAttackExceedEndureLog(string self)
        {
            return Utils.GetRandomElement(AttackExceedEndureLog)
                .Replace("<self>", self);
        }
        public List<string> FallIntoStunLog = new List<string>
        {
            "<self>昏了过去。",
        };

        public string GetFallIntoStunLog(string self)
        {
            return Utils.GetRandomElement(FallIntoStunLog)
                .Replace("<self>", self);
        }

        public List<string> RecoverFromStunLog = new List<string>
        {
            "<self>从昏迷中醒了过来。",
        };

        public string GetRecoverFromStunLog(string self)
        {
            return Utils.GetRandomElement(RecoverFromStunLog)
                .Replace("<self>", self);
        }

        public string CannotFindPathLog = "你不知道如何到达这里。";

        public List<string> EatUpItemLog = new List<string>
        {
            "<item>被<self>吃光了。",
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
            "<self>的<attackType>击中了<target>的<targetPart>。",
            "<target>被<self>的<attackType>击中了<targetPart>。",
        };

        public string GetAttackLog(string self, string target, string targetPart, string attackType)
        {
            return Utils.GetRandomElement(AttackLog)
                .Replace("<self>", self)
                .Replace("<target>", target)
                .Replace("<targetPart>", targetPart)
                .Replace("<attackType>", attackType);
        }

        public List<string> AttackEmptyLog = new List<string>
        {
            "<self>没有击中任何东西。",
            "<self>的攻击落空了。"
        };


        public string GetAttackEmptyLog(string self)
        {
            return Utils.GetRandomElement(AttackEmptyLog)
                .Replace("<self>", self);
        }
    }
}