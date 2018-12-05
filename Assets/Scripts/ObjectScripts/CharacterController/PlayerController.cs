using UnityEngine;
using UtilScripts;

namespace ObjectScripts.CharacterController
{
    public class PlayerController: CharacterController
    {
        private void Update()
        {
            if (!IsTurn()) return;
            if (Character.SpriteController.IsMoving()) return;
            
            
            if (Input.GetKey(KeyCode.A))
            {
                Walk(Direction.Left);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                Walk(Direction.Right);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                Walk(Direction.Down);
            }
            else if (Input.GetKey(KeyCode.W))
            {
                Walk(Direction.Up);
            }
            else if (Input.GetKey(KeyCode.Space))
            {
                Wait();   
            }
            else
            {
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
}