using UnityEngine;
using UtilScripts;

namespace ObjectScripts.CharacterController
{
    public class AiController: CharacterController
    {
        private void Update()
        {
            if (!IsTurn()) return;
            var direction = AStarFinder(Character.GlobalCoord + new Vector2Int(10, 0));
            if (direction == Vector2Int.zero)
            {
                Walk((Direction) Utils.ProcessRandom.Next(4));
            }
            else
            {
                Walk(Utils.VectorToDirection(direction));
                
            }
        }
    }
}