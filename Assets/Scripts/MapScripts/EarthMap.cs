using System.Collections.Generic;
using UtilScripts;

namespace MapScripts
{
    public class EarthMap
    {
        private readonly int[,] _map;
        private readonly int _width;
        private readonly int _height;

        public EarthMap(int width, int height)
        {
            this._width = width;
            this._height = height;
            _map = new int[width, height];
        }

        public int GetMap(EarthMapCoord coord)
        {
            return _map[coord.GetX(), coord.GetY()];
        }

        public void SetMap(EarthMapCoord coord, int value)
        {
            _map[coord.GetX(), coord.GetY()] = value;
        }

        public IEnumerable<EarthMapCoord> IterateMap()
        {
            return EarthMapCoord.IterateCoords(0, _width, 0, _height);
        }
    }
}