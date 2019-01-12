using System.Collections.Generic;
using System.Net;
using ObjectScripts.ActionScripts;
using ObjectScripts.CharacterController.PlayerOrder;
using ObjectScripts.CharSubstance;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UtilScripts;

namespace ObjectScripts.CharacterController
{
    public class PlayerController : CharacterController
    {
        public BaseOrder CurrentOrder;

        [HideInInspector] public Vector2Int TargetCoord;
        [HideInInspector] public Character TargetCharacter;
        [HideInInspector] public Direction TargetDirection;

        private Vector2Int _vecInt;

        public HashSet<Vector2Int> VisibleCoord = new HashSet<Vector2Int>();
        private Vector2Int? _preCoord = null;

        protected override void Awake()
        {
            base.Awake();
        }

        private void LateUpdate()
        {
//            
//            if (Input.GetKey(KeyCode.LeftShift))
//            {
//                if (Input.GetKey(KeyCode.A))
//                {
//                    AttackNeighbourAction.TargetDirection = Direction.Left;
//                    NextAction = AttackNeighbourAction;
//                    CurrentOrder = null;
//                }
//                else if (Input.GetKey(KeyCode.D))
//                {
//                    AttackNeighbourAction.TargetDirection = Direction.Right;
//                    NextAction = AttackNeighbourAction;
//                    CurrentOrder = null;
//                }
//                else if (Input.GetKey(KeyCode.S))
//                {
//                    AttackNeighbourAction.TargetDirection = Direction.Down;
//                    NextAction = AttackNeighbourAction;
//                    CurrentOrder = null;
//                }
//                else if (Input.GetKey(KeyCode.W))
//                {
//                    AttackNeighbourAction.TargetDirection = Direction.Up;
//                    NextAction = AttackNeighbourAction;
//                    CurrentOrder = null;
//                }
//                else
//                {
//                    NextAction = null;
//                }
//            }
//            else
//            {
//                if (Input.GetKey(KeyCode.A))
//                {
//                    WalkAction.TargetDirection = Direction.Left;
//                    NextAction = WalkAction;
//                    CurrentOrder = null;
//                }
//                else if (Input.GetKey(KeyCode.D))
//                {
//                    WalkAction.TargetDirection = Direction.Right;
//                    NextAction = WalkAction;
//                    CurrentOrder = null;
//                }
//                else if (Input.GetKey(KeyCode.S))
//                {
//                    WalkAction.TargetDirection = Direction.Down;
//                    NextAction = WalkAction;
//                    CurrentOrder = null;
//                }
//                else if (Input.GetKey(KeyCode.W))
//                {
//                    WalkAction.TargetDirection = Direction.Up;
//                    NextAction = WalkAction;
//                    CurrentOrder = null;
//                }
//                else if (Input.GetKey(KeyCode.Space))
//                {
//                    NextAction = WaitAction;
//                    CurrentOrder = null;
//                }
//                else
//                {
//                    NextAction = null;
//                }
//            }
            
            UpdateVisual();
        }

        public override void SetAction(BaseAction action)
        {
            base.SetAction(action);
            CurrentOrder = null;
        }
        
        private IEnumerable<Vector2Int> GetVisibleCoord()
        {
            for (var x = -20; x <= 20; x++)
            for (var y = -20; y <= 20; y++)
            {
                var coord = new Vector2Int(x, y) + Character.WorldCoord;
                if (!Character.IsVisible(coord)) continue;
                VisibleCoord.Add(coord);
                yield return coord;
            }
        }

        public void UpdateVisual()
        {
            if (Character.WorldCoord == _preCoord) return;
            foreach (var coord in VisibleCoord)
            {
                foreach (var tilemap in SceneManager.Instance.WorldCoordToTilemap(coord))
                {
                    tilemap.SetColor(tilemap.WorldToCell(SceneManager.Instance.WorldCoordToPos(coord)), Color.gray);   
                    _preCoord = Character.WorldCoord;
                }
            }

            foreach (var coord in GetVisibleCoord())
            {
                foreach (var tilemap in SceneManager.Instance.WorldCoordToTilemap(coord))
                {
                    tilemap.SetColor(tilemap.WorldToCell(SceneManager.Instance.WorldCoordToPos(coord)), Color.white);   
                    _preCoord = Character.WorldCoord;
                }
            }
        }

        public override void UpdateFunction()
        {
            if (CurrentOrder != null) CurrentOrder = CurrentOrder.DoOrder();
            if (NextAction == null) return;
            UpdateVisual();
            NextAction.DoAction();
            NextAction = null;
        }
    }
}