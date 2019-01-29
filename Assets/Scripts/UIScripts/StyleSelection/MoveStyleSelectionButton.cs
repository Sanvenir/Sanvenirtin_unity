using System.Collections.Generic;
using UtilScripts;

namespace UIScripts.StyleSelection
{
    public class MoveStyleSelectionButton: StyleSelectButton
    {
        protected override IEnumerable<INamed> GetStyleSelections()
        {
            yield break;
        }
    }
}