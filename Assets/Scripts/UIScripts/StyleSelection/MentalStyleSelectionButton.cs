using System.Collections.Generic;
using ObjectScripts.StyleScripts.MentalStyleScripts;
using UtilScripts;

namespace UIScripts.StyleSelection
{
    public class MentalStyleSelectionButton : StyleSelectButton
    {
        protected override IEnumerable<INamed> GetStyleSelections()
        {
            foreach (var mentalStyle in Player.MentalStyleList)
            {
                if (mentalStyle == Player.CurrentMentalStyle) continue;
                yield return mentalStyle;
            }
        }

        protected override void Selected(INamed selection)
        {
            PlayerController.ChangeMentalStyle(selection as BaseMentalStyle);
        }
    }
}