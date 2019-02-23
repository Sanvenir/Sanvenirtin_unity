using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using ObjectScripts.CharacterController.PlayerOrder;
using ObjectScripts.CharSubstance;

namespace ObjectScripts.StyleScripts.ActStyleScripts
{
    [Serializable]
    public class BaseActStyle: BaseStyle
    {
        protected BaseActStyle(Character self) : base(self)
        {
        }

        public List<ActActionSkill> ActSkillList;
        public string TextName;
        
        public override string GetTextName()
        {
            return TextName;
        }
    }
}