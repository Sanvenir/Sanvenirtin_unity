using System;
using System.Collections.Generic;
using ExceptionScripts;
using UnityEngine;

namespace UtilScripts
{
    public static class Utils
    {
        public static System.Random ProcessRandom;

        public static int FloatToInt(float num)
        {
            return (int) Math.Floor(num);
        }

        public static int Div(int num, int divisor)
        {
            return num >= 0 ? (num / divisor) : ((num + 1) / divisor - 1);
        }

        public static int Mod(int num, int divisor)
        {
            return num >= 0 ? (num % divisor) : (num % divisor + divisor);
        }

        public static void DirectionToIncrement(
            Direction direction, out int dx, out int dy)
        {
            switch (direction)
            {
                case Direction.Up:
                    dx = 0;
                    dy = 1;
                    break;
                case Direction.Down:
                    dx = 0;
                    dy = -1;
                    break;
                case Direction.Left:
                    dx = -1;
                    dy = 0;
                    break;
                case Direction.Right:
                    dx = 1;
                    dy = 0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("direction", direction, null);
            }
        }

        public static Direction VectorToDirection(Vector2Int delta)
        {
            return IncrementToDirection(delta.x, delta.y);
        }

        public static Vector2Int DirectionToVector(Direction direction)
        {
            int dx, dy;
            DirectionToIncrement(direction, out dx, out dy);
            return new Vector2Int(dx, dy);
        }

        public static Direction IncrementToDirection(float dx, float dy)
        {
            if (dx > dy && dx >= -dy)
            {
                return Direction.Left;
            }

            if (dy > -dx && dy >= dx)
            {
                return Direction.Up;
            }

            if (-dx > -dy && -dx >= dy)
            {
                return Direction.Down;
            }

            if (-dy > dx && -dy >= -dx)
            {
                return Direction.Right;
            }

            throw new GameException("Params not in the range -1..1");
        }

        public static IEnumerable<Vector2Int> GetNeighbours(Vector2Int coord)
        {
            yield return coord + Vector2Int.up;
            yield return coord + Vector2Int.down;
            yield return coord + Vector2Int.left;
            yield return coord + Vector2Int.right;
        }

        public static Vector2Int Vector3IntTo2(Vector3Int vector)
        {
            return new Vector2Int(vector.x, vector.y);
        }

        public static Vector3Int Vector2IntTo3(Vector2Int vector)
        {
            return new Vector3Int(vector.x, vector.y, 0);
        }
    }
}