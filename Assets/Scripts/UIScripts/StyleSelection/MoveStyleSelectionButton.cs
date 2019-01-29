using System.Collections.Generic;

namespace UIScripts.StyleSelection
{
    public class MoveStyleSelectionButton: StyleSelectButton
    {
        protected override IEnumerable<object> GetStyleSelections()
        {
            yield return "步行中";
            yield return "跑步中";
            yield return "冲刺中";
        }
    }
}