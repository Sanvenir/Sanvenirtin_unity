using System;
using System.Collections.Generic;
using ObjectScripts.CharSubstance;

namespace ObjectScripts.StyleScripts.ActStyleScripts
{
    [Serializable]
    public class BaseActStyle : BaseStyle
    {
        public List<ActActionSkill> ActSkillList;
        public string TextName;

        protected BaseActStyle(Character self) : base(self)
        {
        }

        public override string GetTextName()
        {
            return TextName;
        }
    }
}