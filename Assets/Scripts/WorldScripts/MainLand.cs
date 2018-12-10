using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine.Audio;
using UtilScripts;

namespace MapScripts
{
    public class MainLand : IComparable<MainLand>
    {
        public List<EarthMapCoord> Coords;
        public HashSet<EarthMapCoord> EdgeCoords;
        public int LandSize;
        
        private static EarthMap _earthMap;

        public static void SetMap(EarthMap map)
        {
            _earthMap = map;
        }

        public MainLand(List<EarthMapCoord> coords)
        {
            Coords = coords;
            LandSize = Coords.Count;
            
            EdgeCoords = new HashSet<EarthMapCoord>();

            foreach (var coord in Coords)
            {
                if (coord.GetNeighbourCoords().Any(nextTile => _earthMap.GetMap(nextTile) == 0))
                {
                    EdgeCoords.Add(coord);
                }
            }
        }

        public int CompareTo(MainLand otherLand)
        {
            return otherLand.LandSize.CompareTo(LandSize);
        }
    }
}