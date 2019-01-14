using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Random = System.Random;

namespace UtilScripts
{
    public static class Utils
    {
        public static Random ProcessRandom;

        public static int FloatToInt(float num)
        {
            return (int) Mathf.Floor(num);
        }

        public static int Div(int num, int divisor)
        {
            return num >= 0 ? num / divisor : (num + 1) / divisor - 1;
        }

        public static int Mod(int num, int divisor)
        {
            return num >= 0 ? num % divisor : num % divisor + divisor;
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
                case Direction.None:
                    dx = 0;
                    dy = 0;
                    break;
                default:
                    dx = 0;
                    dy = 0;
                    break;
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
            if (dx > dy && dx >= -dy) return Direction.Right;

            if (dy > -dx && dy >= dx) return Direction.Up;

            if (-dx > -dy && -dx >= dy) return Direction.Left;

            if (-dy > dx && -dy >= -dx) return Direction.Down;

            return Direction.None;
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

        public static Vector2 Vector3To2(Vector3 vector3)
        {
            return vector3;
        }

        public static Vector3 Vector2To3(Vector2 vector2)
        {
            return vector2;
        }

        /// <summary>
        ///     Randomly shift a position
        /// </summary>
        /// <param name="position">Original position</param>
        /// <param name="offset">Max offset</param>
        /// <returns>Shifted position</returns>
        public static Vector2 GetRandomShiftPosition(Vector2 position, float offset = 0.2f)
        {
            return position + new Vector2(
                       (float) ProcessRandom.NextDouble() * offset * 2 - offset,
                       (float) ProcessRandom.NextDouble() * offset * 2 - offset);
        }

        public static Dictionary<TKey, TValue> CloneDictionaryCloningValues<TKey, TValue>
            (Dictionary<TKey, TValue> original) where TValue : ICloneable
        {
            var ret = new Dictionary<TKey, TValue>(original.Count,
                original.Comparer);
            foreach (var entry in original) ret.Add(entry.Key, (TValue) entry.Value.Clone());

            return ret;
        }

        public static List<TValue> CloneDictionaryCloningValues<TValue>
            (IEnumerable<TValue> original) where TValue : ICloneable
        {
            return original.Select(entry => (TValue) entry.Clone()).ToList();
        }

        public static T GetRandomElement<T>(List<T> list)
        {
            if (list == null || list.Count == 0)
                return default(T);
            return list[ProcessRandom.Next(list.Count)];
        }

//        public static void ProgressBar (float value, string label)
//        {
//            // Use a rect for the progress bar using the same margins as a textfield:
//            Rect rect = GUILayoutUtility.GetRect (18, 18, "TextField");
//            EditorGUI.ProgressBar (rect, value, label);
//            EditorGUILayout.Space ();
//        }
    }
}