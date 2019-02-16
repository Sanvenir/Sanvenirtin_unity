using System.Collections.Generic;
using ObjectScripts.CharSubstance;
using ObjectScripts.StyleScripts.MoveStyleScripts;
using UtilScripts;

namespace UIScripts.StyleSelection
{
    public class MoveStyleSelectionButton: StyleSelectButton
    {
        
        protected override IEnumerable<INamed> GetStyleSelections()
        {
            foreach (var moveStyle in Player.MoveStyleList)
            {
                if (moveStyle == Player.CurrentMoveStyle) continue;
                yield return moveStyle;
            }
        }

        protected override void Selected(INamed selection)
        {
            PlayerController.ChangeMoveStyle(selection as BaseMoveStyle);
        }
    }
}