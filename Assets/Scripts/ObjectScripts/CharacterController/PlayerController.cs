using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using AreaScripts;
using ObjectScripts.ActionScripts;
using ObjectScripts.CharacterController.PlayerOrder;
using ObjectScripts.CharSubstance;
using ObjectScripts.StyleScripts.ActStyleScripts;
using ObjectScripts.StyleScripts.MaradyStyleScripts;
using ObjectScripts.StyleScripts.MentalStyleScripts;
using ObjectScripts.StyleScripts.MoveStyleScripts;
using UIScripts.StyleSelection;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.Tilemaps;
using UtilScripts;

namespace ObjectScripts.CharacterController
{
    public class PlayerController : CharacterController
    {
        public BaseOrder CurrentOrder;

        [HideInInspector] public Vector2Int TargetCoord;
        [HideInInspector] public Character TargetCharacter;
        [HideInInspector] public Direction TargetDirection;

        public MoveStyleSelectionButton MoveStyleSelectionButton;
        public MentalStyleSelectionButton MentalStyleSelectionButton;
        public ActStyleSelectionButton ActStyleSelectionButton;

        private Vector2Int _vecInt;

        public HashSet<Vector2Int> MemorizedCoord = new HashSet<Vector2Int>();
        private Vector2Int? _preCoord = null;

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
                yield return coord;
            }
        }

        public IEnumerator SetColor(TilemapTerrain tilemap, Vector2Int coord, bool isMemorized)
        {
            var cell = tilemap.Tilemap.WorldToCell(SceneManager.Instance.WorldCoordToPos(coord));
            if(tilemap == null) yield break;
            var color = tilemap.Tilemap.GetColor(cell);
            var targetColor = isMemorized ? Color.gray : Color.white;
            if(color == targetColor) yield break;
            var time = SceneManager.Instance.GetUpdateTime() / 2;
            var incColor = targetColor / time;
            for (var i = 0; i < time; i++)
            {
                color += incColor;
                if(tilemap == null) yield break;
                tilemap.Tilemap.SetColor(cell, color);
                yield return null;
            }

            if(tilemap == null) yield break;
            tilemap.Tilemap.SetColor(cell, targetColor);

        }

        public void UpdateVisual()
        {
            
            if (Character.WorldCoord == _preCoord) return;
            var buff = new HashSet<Vector2Int>();
            foreach (var coord in MemorizedCoord)
            {
                var tilemap = SceneManager.Instance.WorldCoordToTilemap(coord);
                if (tilemap == null) continue;
                _preCoord = Character.WorldCoord;
                StartCoroutine(SetColor(tilemap, coord, true));
                buff.Add(coord);
            }

            MemorizedCoord = buff;

            foreach (var coord in GetVisibleCoord())
            {
                var tilemap = SceneManager.Instance.WorldCoordToTilemap(coord);
                if (tilemap == null) continue;
                _preCoord = Character.WorldCoord;
                StartCoroutine(SetColor(tilemap, coord, false));
                MemorizedCoord.Add(coord);
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

        public override void ChangeActStyle(BaseActStyle actStyle)
        {
            base.ChangeActStyle(actStyle);
            ActStyleSelectionButton.Text.text = Character.CurrentActStyle.GetTextName();
        }

        public override void ChangeMaradyStyle(BaseMaradyStyle maradyStyle)
        {
            base.ChangeMaradyStyle(maradyStyle);
        }

        public override void ChangeMentalStyle(BaseMentalStyle mentalStyle)
        {
            base.ChangeMentalStyle(mentalStyle);
            MentalStyleSelectionButton.Text.text = Character.CurrentMentalStyle.GetTextName();
        }

        public override void ChangeMoveStyle(BaseMoveStyle moveStyle)
        {
            base.ChangeMoveStyle(moveStyle);
            MoveStyleSelectionButton.Text.text = Character.CurrentMoveStyle.GetTextName();
        }
    }
}