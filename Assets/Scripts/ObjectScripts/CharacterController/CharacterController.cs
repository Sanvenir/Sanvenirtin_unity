using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UtilScripts;

namespace ObjectScripts.CharacterController
{
    public abstract class CharacterController: MonoBehaviour
    {
        protected Character Character;

        private void Awake()
        {
            Character = gameObject.GetComponent<Character>();
        }

        public abstract void UpdateFunction();

        private void Update()
        {
            Character.RefreshProperties();
            if (Character.Dead) return;
            if (!Character.IsTurn()) return;
            UpdateFunction();
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
            if (target == Character.GlobalCoord)
            {
                return Vector2Int.zero;
            }

            var hValue = (target - Character.GlobalCoord).sqrMagnitude;
            if (hValue > memorySize * memorySize)
            {
                return SimpleFinder(target);
            }

            var openList = new SortedList<AStarNode, Vector2Int>();
            var closedSet = new HashSet<Vector2Int>();
            var node = new AStarNode(0, hValue, Character.GlobalCoord);

            openList.Add(node, Character.GlobalCoord);

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
                    if (Character.GetColliderAtGlobalCoord(neighbour) != null ||
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
            if (target == Character.GlobalCoord)
            {
                return Vector2Int.zero;
            }

            var dx = target.x - Character.GlobalCoord.x;
            var dy = target.y - Character.GlobalCoord.y;
            if (dx != 0)
            {
                dx = dx.CompareTo(0);
            }

            if (dy != 0)
            {
                dy = dy.CompareTo(0);
            }

            if ((dx & dy) == 0)
                return Character.GetColliderAtGlobalCoord(new Vector2Int(dx, dy) + Character.GlobalCoord) == null
                    ? new Vector2Int(dx, dy)
                    : Vector2Int.zero;
            var delta = new Vector2Int();
            var altDelta = new Vector2Int();
            if (Utils.ProcessRandom.Next(2) == 0)
            {
                delta.x = dx;
                altDelta.y = dy;
            }
            else
            {
                delta.y = dy;
                altDelta.x = dx;
            }

            if (Character.GetColliderAtGlobalCoord(delta + Character.GlobalCoord) == null)
            {
                return delta;
            }

            return Character.GetColliderAtGlobalCoord(altDelta + Character.GlobalCoord) == null ? altDelta : Vector2Int.zero;
        }

    }
}