using System;
using System.Collections.Generic;
using ExceptionScripts;
using UnityEngine;
using System.Linq;
using DefaultNamespace;

namespace UtilScripts
{
    [Serializable]
    public struct EarthMapCoord
    {
        private readonly int _mapX;
        private readonly int _mapY;

        public static EarthMapCoord operator +(EarthMapCoord coord1, EarthMapCoord coord2)
        {
            return new EarthMapCoord(
                coord1._mapX + coord2._mapX,
                coord1._mapY + coord2._mapY
            );
        }

        public EarthMapCoord GetDeltaCoord(int dx, int dy)
        {
            return new EarthMapCoord(_mapX + dx, _mapY + dy);
        }

        public EarthMapCoord(int x, int y)
        {
            _mapX = Utils.Mod(x, EarthMapManager.MapWidth);
            if (y >= EarthMapManager.MapHeight || y < 0)
            {
                throw new CoordOutOfWorldException("Y is out of the range of the world map");
            }

            _mapY = y;
        }

        public static EarthMapCoord CreateFromIdentity(int identity)
        {
            if (identity < 0)
            {
                throw new GameException(
                    "If identity is negative, it must not be a area under the world map");
            }

            return new EarthMapCoord(
                identity % EarthMapManager.MapWidth,
                identity / EarthMapManager.MapWidth);
        }

        public int GetIdentity()
        {
            return _mapY * EarthMapManager.MapWidth + _mapX;
        }

        public int GetX()
        {
            return _mapX;
        }

        public int GetY()
        {
            return _mapY;
        }

        public bool Equals(EarthMapCoord coord)
        {
            return (GetX() == coord.GetX()) && (GetY() == coord.GetY());
        }

        public IEnumerable<EarthMapCoord> GetSurroundCoords(int delta = 1)
        {
            // Get the 8 coords around the given coord.
            var tmpThis = this;
            return IterateCoords(
                tmpThis.GetX() - delta, tmpThis.GetX() + delta + 1,
                tmpThis.GetY() - delta, tmpThis.GetY() + delta + 1).Where(coord => !tmpThis.Equals(coord));
        }

        public IEnumerable<EarthMapCoord> GetNeighbourCoords()
        {
            // Get the 4 coords next to the given coord.
            yield return new EarthMapCoord(GetX() - 1, GetY());
            yield return new EarthMapCoord(GetX() + 1, GetY());
            if (GetY() != EarthMapManager.MapHeight - 1)
            {
                yield return new EarthMapCoord(GetX(), GetY() + 1);
            }

            if (GetY() != 0)
            {
                yield return new EarthMapCoord(GetX(), GetY() - 1);
            }
        }

        public static IEnumerable<EarthMapCoord> IterateCoords(
            int startX, int endX, int startY, int endY)
        {
            startY = Math.Max(startY, 0);
            endY = Math.Min(endY, EarthMapManager.MapHeight);
            for (var x = startX; x < endX; x++)
            for (var y = startY; y < endY; y++)
            {
                yield return new EarthMapCoord(x, y);
            }
        }
    }
}