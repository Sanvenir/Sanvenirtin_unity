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
        public List<WorldCoord> Coords;
        public HashSet<WorldCoord> EdgeCoords;
        public int LandSize;
        
        private static WorldMap _worldMap;

        public static void SetMap(WorldMap map)
        {
            _worldMap = map;
        }

        public MainLand(List<WorldCoord> coords)
        {
            Coords = coords;
            LandSize = Coords.Count;
            
            EdgeCoords = new HashSet<WorldCoord>();

            foreach (var coord in Coords)
            {
                if (coord.GetNeighbourCoords().Any(nextTile => _worldMap.GetMap(nextTile) == 0))
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