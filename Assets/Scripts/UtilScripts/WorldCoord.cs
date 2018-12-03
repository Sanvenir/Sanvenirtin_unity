using System;
using System.Collections.Generic;
using ExceptionScripts;
using UnityEngine;
using System.Linq;
using DefaultNamespace;

namespace UtilScripts
{
    [Serializable]
    public struct WorldCoord
    {
        private readonly int _worldX;
        private readonly int _worldY;

        public static WorldCoord operator +(WorldCoord coord1, WorldCoord coord2)
        {
            return new WorldCoord(
                coord1._worldX + coord2._worldX,
                coord1._worldY + coord2._worldY
            );
        }

        public WorldCoord GetDeltaCoord(int dx, int dy)
        {
            return new WorldCoord(_worldX + dx, _worldY + dy);
        }

        public WorldCoord(int x, int y)
        {
            _worldX = Utils.Mod(x, WorldManager.WorldWidth);
            if (y >= WorldManager.WorldHeight || y < 0)
            {
                throw new CoordOutOfWorldException("Y is out of the range of the world map");
            }

            _worldY = y;
        }

        public static WorldCoord CreateFromIdentity(int identity)
        {
            if (identity < 0)
            {
                throw new GameException(
                    "If identity is negative, it must not be a area under the world map");
            }

            return new WorldCoord(
                identity % WorldManager.WorldWidth,
                identity / WorldManager.WorldWidth);
        }

        public int GetIdentity()
        {
            return _worldY * WorldManager.WorldWidth + _worldX;
        }

        public int GetX()
        {
            return _worldX;
        }

        public int GetY()
        {
            return _worldY;
        }

        public bool Equals(WorldCoord coord)
        {
            return (GetX() == coord.GetX()) && (GetY() == coord.GetY());
        }

        public IEnumerable<WorldCoord> GetSurroundCoords(int delta = 1)
        {
            // Get the 8 coords around the given coord.
            var tmpThis = this;
            return IterateCoords(
                tmpThis.GetX() - delta, tmpThis.GetX() + delta + 1,
                tmpThis.GetY() - delta, tmpThis.GetY() + delta + 1).Where(coord => !tmpThis.Equals(coord));
        }

        public IEnumerable<WorldCoord> GetNeighbourCoords()
        {
            // Get the 4 coords next to the given coord.
            yield return new WorldCoord(GetX() - 1, GetY());
            yield return new WorldCoord(GetX() + 1, GetY());
            if (GetY() != WorldManager.WorldHeight - 1)
            {
                yield return new WorldCoord(GetX(), GetY() + 1);
            }

            if (GetY() != 0)
            {
                yield return new WorldCoord(GetX(), GetY() - 1);
            }
        }

        public static IEnumerable<WorldCoord> IterateCoords(
            int startX, int endX, int startY, int endY)
        {
            startY = Math.Max(startY, 0);
            endY = Math.Min(endY, WorldManager.WorldHeight);
            for (var x = startX; x < endX; x++)
            for (var y = startY; y < endY; y++)
            {
                yield return new WorldCoord(x, y);
            }
        }
    }
}