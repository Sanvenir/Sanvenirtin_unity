using System;
using System.Collections.Generic;
using System.Linq;
using UtilScripts;

namespace MapScripts
{
    /// <summary>
    ///     The Main Land of the Earth Map
    /// </summary>
    public class MainLand : IComparable<MainLand>
    {
        private static EarthMap _earthMap;
        public List<EarthMapCoord> Coords;
        public HashSet<EarthMapCoord> EdgeCoords;
        public int LandSize;

        public MainLand(List<EarthMapCoord> coords)
        {
            Coords = coords;
            LandSize = Coords.Count;

            EdgeCoords = new HashSet<EarthMapCoord>();

            foreach (var coord in Coords)
                if (coord.GetNeighbourCoords().Any(nextTile => _earthMap.GetMap(nextTile) == 0))
                    EdgeCoords.Add(coord);
        }

        public int CompareTo(MainLand otherLand)
        {
            return otherLand.LandSize.CompareTo(LandSize);
        }

        public static void SetMap(EarthMap map)
        {
            _earthMap = map;
        }
    }
}