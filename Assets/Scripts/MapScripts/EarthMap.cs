using System.Collections.Generic;
using UtilScripts;

namespace MapScripts
{
    /// <summary>
    ///     The Earth map of a game world
    /// </summary>
    public class EarthMap
    {
        private readonly int _height;

        /// <summary>
        ///     Types of local areas on the Earth Map
        /// </summary>
        private readonly int[,] _map;

        private readonly int _width;

        /// <summary>
        ///     Constructor of a Earth Map
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public EarthMap(int width, int height)
        {
            _width = width;
            _height = height;
            _map = new int[width, height];
        }

        /// <summary>
        ///     Getter of given Earth Map Coord
        /// </summary>
        /// <param name="coord"></param>
        /// <returns>Type of the area</returns>
        public int GetMap(EarthMapCoord coord)
        {
            return _map[coord.GetX(), coord.GetY()];
        }

        /// <summary>
        ///     Setter of given Earth Map Coord
        /// </summary>
        /// <param name="coord"></param>
        /// <param name="value"></param>
        public void SetMap(EarthMapCoord coord, int value)
        {
            _map[coord.GetX(), coord.GetY()] = value;
        }

        /// <summary>
        ///     Iterate all Earth Map coords
        /// </summary>
        /// <returns></returns>
        public IEnumerable<EarthMapCoord> IterateMap()
        {
            return EarthMapCoord.IterateCoords(0, _width, 0, _height);
        }
    }
}