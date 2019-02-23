using System.Collections.Generic;
using ObjectScripts.StyleScripts.ActStyleScripts;
using UtilScripts;

namespace UIScripts.StyleSelection
{
    public class ActStyleSelectionButton : StyleSelectButton
    {
        protected override IEnumerable<INamed> GetStyleSelections()
        {
            foreach (var actStyle in Player.ActStyleList)
            {
                if (actStyle == Player.CurrentActStyle) continue;
                yield return actStyle;
            }
        }

        protected override void Selected(INamed selection)
        {
            PlayerController.ChangeActStyle(selection as BaseActStyle);
        }
    }
}