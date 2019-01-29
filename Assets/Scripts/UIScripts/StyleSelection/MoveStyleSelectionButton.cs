using System.Collections.Generic;
using ObjectScripts.CharSubstance;
using ObjectScripts.StyleScripts.MoveStyleScripts;
using UtilScripts;

namespace UIScripts.StyleSelection
{
    public class MoveStyleSelectionButton: StyleSelectButton
    {
        private static Character Player
        {
            get { return PlayerController.Character; }
        }
        
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
            var result = selection as BaseMoveStyle;
            if (result == null) return;
            Player.CurrentMoveStyle = result;
            Text.text = Player.CurrentMoveStyle.GetTextName();
        }
    }
}