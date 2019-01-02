using System;
using System.Collections.Generic;

namespace ObjectScripts.BodyPartScripts
{
    [Serializable]
    public class BodyPartList
    {
        public string Name;
        public List<BasicItem.BasicItem> Components;
    }
}