using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using ObjectScripts.ActionScripts;
using ObjectScripts.CharSubstance;
using ObjectScripts.StyleScripts.ActStyleScripts;
using ObjectScripts.StyleScripts.MaradyStyleScripts;
using ObjectScripts.StyleScripts.MentalStyleScripts;
using ObjectScripts.StyleScripts.MoveStyleScripts;
using UIScripts;
using UnityEngine;
using UtilScripts;

namespace ObjectScripts.CharacterController
{
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public abstract class CharacterController : MonoBehaviour
    {
        private int _recoveredTime;

        public Character Character;

        /// <summary>
        ///     Actions
        /// </summary>
        protected BaseAction NextAction;

        public CharacterTextDialog TextDialog;

        public List<ActActionSkill> ActSetterList
        {
            get { return Character.CurrentActStyle == null ? null : Character.CurrentActStyle.ActSkillList; }
        }

        /// <summary>
        /// </summary>
        /// <param name="action"></param>
        public virtual void SetAction(BaseAction action)
        {
            NextAction = action;
        }

        protected virtual void Start()
        {
            if (TextDialog == null) return;
            TextDialog = Instantiate(TextDialog, Character.SpriteRenderer.transform);
            TextDialog.transform.localPosition = Vector3.up * Character.SpriteRenderer.size.y;
            TextDialog.Character = Character;
        }

        public abstract void UpdateFunction();

        private void Update()
        {
            if (!Character.IsTurn()) return;
            Character.RefreshProperties();
            if (Character.Dead) return;
            if (Character.Stun.Value)
            {
                new WaitAction(Character).DoAction();
                return;
            }

            UpdateFunction();
        }

        public virtual void GetHostility(Character hostility, int level)
        {
        }

        public void ClearMessage()
        {
            if (TextDialog == null) return;
            TextDialog.Clear();
        }

        public void PrintMessage(string message)
        {
            SceneManager.Instance.Print(message, Character.WorldCoord);
            if (TextDialog == null) return;
            TextDialog.PrintDialog(message);
        }

        public virtual void ChangeMoveStyle(BaseMoveStyle moveStyle)
        {
            if (moveStyle == null) return;
            Character.CurrentMoveStyle = moveStyle;
        }

        public virtual void ChangeActStyle(BaseActStyle actStyle)
        {
            if (actStyle == null) return;
            Character.CurrentActStyle = actStyle;
        }

        public virtual void ChangeMentalStyle(BaseMentalStyle mentalStyle)
        {
            if (mentalStyle == null) return;
            Character.CurrentMentalStyle = mentalStyle;
        }

        public virtual void ChangeMaradyStyle(BaseMaradyStyle maradyStyle)
        {
            if (maradyStyle == null) return;
            Character.CurrentMaradyStyle = maradyStyle;
        }

        public Vector2Int AStarFinder(Vector2Int target, int memorySize = 50)
        {
            if (target == Character.WorldCoord) return Vector2Int.zero;

            var hValue = (target - Character.WorldCoord).sqrMagnitude;
            if (hValue > memorySize * memorySize) return SimpleFinder(target);

            var openList = new SortedList<AStarNode, Vector2Int>();
            var closedSet = new HashSet<Vector2Int>();
            var node = new AStarNode(0, hValue, Character.WorldCoord);

            openList.Add(node, Character.WorldCoord);

            while (openList.Count > 0 && memorySize > 0)
            {
                memorySize--;
                node = openList.Keys[0];
                openList.Remove(node);

                closedSet.Add(node.Coord);
                foreach (var neighbour in Utils.GetNeighbours(node.Coord))
                {
                    if (neighbour == target)
                    {
                        var result = neighbour - node.Coord;
                        while (node.Parent != null)
                        {
                            result = node.Coord - node.Parent.Coord;
                            node = node.Parent;
                        }

                        return result;
                    }

                    // TODO: Increment of GValue need to refactor according to ground properties
                    var newGValue = node.GValue + 1;
                    if (!Character.CheckColliderAtWorldCoord(neighbour) ||
                        closedSet.Contains(neighbour)) continue;

                    if (openList.ContainsValue(neighbour))
                    {
                        var neighbourNode = openList.Keys[openList.IndexOfValue(neighbour)];
                        if (neighbourNode.GValue <= newGValue) continue;
                        openList.Remove(neighbourNode);
                        neighbourNode.GValue = newGValue;
                        neighbourNode.FValue = newGValue + neighbourNode.HValue;
                        neighbourNode.Parent = node;
                        openList.Add(neighbourNode, neighbour);
                    }
                    else
                    {
                        try
                        {
                            openList.Add(new AStarNode(
                                newGValue,
                                (target - neighbour).sqrMagnitude,
                                neighbour,
                                node), neighbour);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            throw;
                        }
                    }
                }
            }

            return SimpleFinder(target);
        }

        public Vector2Int SimpleFinder(Vector2Int target)
        {
            if (target == Character.WorldCoord) return Vector2Int.zero;

//            var dx = target.x - Parent.WorldCoord.x;
//            var dy = target.y - Parent.WorldCoord.y;

            var delta = target - Character.WorldCoord;
            if (delta.x != 0) delta.x = delta.x.CompareTo(0);

            if (delta.y != 0) delta.y = delta.y.CompareTo(0);

            if ((delta.x & delta.y) == 0)
                return Character.CheckColliderAtWorldCoord(delta + Character.WorldCoord)
                    ? delta
                    : Vector2Int.zero;
            // var delta = new Vector2Int();
            var altDelta = new Vector2Int();
            if (Utils.ProcessRandom.Next(2) == 0)
            {
                altDelta.y = delta.y;
                delta.y = 0;
            }
            else
            {
                altDelta.x = delta.x;
                delta.x = 0;
            }

            if (Character.CheckColliderAtWorldCoord(delta + Character.WorldCoord)) return delta;

            return Character.CheckColliderAtWorldCoord(altDelta + Character.WorldCoord) ? altDelta : Vector2Int.zero;
        }


        // Path Finder Algorithms
        public class AStarNode : IComparable<AStarNode>
        {
            private readonly int _hashCode;
            public readonly Vector2Int Coord;
            public readonly int HValue;
            public int FValue;
            public int GValue;
            public AStarNode Parent;

            public AStarNode(
                int gValue, int hValue, Vector2Int coord, [CanBeNull] AStarNode parent = null)
            {
                GValue = gValue;
                HValue = hValue;
                FValue = hValue + gValue;
                Coord = coord;
                Parent = parent;
            }

            public int CompareTo(AStarNode obj)
            {
                if (obj.Coord == Coord) return 0;

                if (obj.FValue != FValue) return FValue.CompareTo(obj.FValue);

                return Coord.x == obj.Coord.x ? Coord.y.CompareTo(obj.Coord.y) : Coord.x.CompareTo(obj.Coord.x);
            }
        }
    }
}