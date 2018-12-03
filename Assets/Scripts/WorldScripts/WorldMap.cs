using System.Collections.Generic;
using UtilScripts;

namespace MapScripts
{
    public class WorldMap
    {
        private readonly int[,] _map;
        private readonly int _width;
        private readonly int _height;

        public WorldMap(int width, int height)
        {
            this._width = width;
            this._height = height;
            _map = new int[width, height];
        }

        public int GetMap(WorldCoord coord)
        {
            return _map[coord.GetX(), coord.GetY()];
        }

        public void SetMap(WorldCoord coord, int value)
        {
            _map[coord.GetX(), coord.GetY()] = value;
        }

        public IEnumerable<WorldCoord> IterateMap()
        {
            return WorldCoord.IterateCoords(0, _width, 0, _height);
        }
    }
                            }