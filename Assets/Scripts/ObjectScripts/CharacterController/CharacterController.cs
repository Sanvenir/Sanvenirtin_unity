using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using ObjectScripts.ActionScripts;
using ObjectScripts.CharSubstance;
using UnityEngine;
using UtilScripts;

namespace ObjectScripts.CharacterController
{
    public abstract class CharacterController: MonoBehaviour
    {
        // Actions
        protected BaseAction NextAction;

        public virtual void SetAction(BaseAction action)
        {
            NextAction = action;
        }
        
        public Character Character;

        private int _recoveredTime = 0;

        protected virtual void Awake()
        {
        }

        public virtual void Initialize()
        {
            
        }

        public abstract void UpdateFunction();

        private void Update()
        {
            Character.RefreshProperties();
            if (Character.Dead) return;
            if (_recoveredTime < SceneManager.Instance.CurrentTime)
            {
                _recoveredTime += Character.Properties.GetReactTime();
                Character.Recovering();
            }
            
            
            if (!Character.IsTurn()) return;
            UpdateFunction();
        }

        public virtual void GetHostility(Character hostility, int level)
        {
            
        }


        // Path Finder Algorithms
        public class AStarNode : IComparable<AStarNode>
        {
            private readonly int _hashCode;
            public readonly Vector2Int Coord;
            public int FValue;
            public int GValue;
            public readonly int HValue;
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
                if (obj.Coord == Coord)
                {
                    return 0;
                }

                if (obj.FValue != FValue)
                {
                    return FValue.CompareTo(obj.FValue);
                }

                return Coord.x == obj.Coord.x ? Coord.y.CompareTo(obj.Coord.y) : Coord.x.CompareTo(obj.Coord.x);
            }
        }

        public Vector2Int AStarFinder(Vector2Int target, int memorySize = 50)
        {
            if (target == Character.WorldCoord)
            {
                return Vector2Int.zero;
            }

            var hValue = (target - Character.WorldCoord).sqrMagnitude;
            if (hValue > memorySize * memorySize)
            {
                return SimpleFinder(target);
            }

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
            if (target == Character.WorldCoord)
            {
                return Vector2Int.zero;
            }

//            var dx = target.x - Character.WorldCoord.x;
//            var dy = target.y - Character.WorldCoord.y;

            var delta = target - Character.WorldCoord;
            if (delta.x != 0)
            {
                delta.x = delta.x.CompareTo(0);
            }

            if (delta.y != 0)
            {
                delta.y = delta.y.CompareTo(0);
            }

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

            if (Character.CheckColliderAtWorldCoord(delta + Character.WorldCoord))
            {
                return delta;
            }

            return Character.CheckColliderAtWorldCoord(altDelta + Character.WorldCoord) ? altDelta : Vector2Int.zero;
        }

    }
}