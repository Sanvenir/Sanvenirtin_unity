using System;
using System.Collections.Generic;

namespace ObjectScripts.BodyPartScripts
{
    /// <summary>
    ///     Used in GameSetting, List the body parts components
    /// </summary>
    [Serializable]
    public class BodyPartList
    {
        /// <summary>
        ///     Component objects make up this type of body parts
        /// </summary>
        public List<SingularObject> Components;

        public string Name;
    }
}