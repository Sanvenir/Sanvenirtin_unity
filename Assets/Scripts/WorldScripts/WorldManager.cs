using System.Collections.Generic;
using MapScripts;
using UnityEngine;
using UtilScripts;

namespace DefaultNamespace
{
    public class WorldManager: MonoBehaviour
    {
        public static int WorldWidth = 200, WorldHeight = 100;
        public static int LocalWidth = 10, LocalHeight = 10;
        
        // Percent of land init ratio
        public int LandPercent = 50;

        // Threshold of the min size of Sea or Land
        public int SeaThresholdSize = 100;
        public int LandThresholdSize = 100;
        
        // Percent of the possibility of erasing lands with small size
        public int LandErasePercent = 70;

        // Time of smooth algorithms
        public int SmoothTime = 5;

        public GameObject[] BaseAreaPrefabs;

        public WorldMap WorldMap;
        
        private readonly List<MainLand> _mainLands = new List<MainLand>();

        public void GenerateMap()
        {
            WorldMap = new WorldMap(WorldWidth, WorldHeight);
            MainLand.SetMap(WorldMap);

            foreach(var coord in WorldMap.IterateMap())
            {
                WorldMap.SetMap(coord, Utils.ProcessRandom.Next(0, 100) < LandPercent ? 1 : 0);
            }
            
            //Smooth the Map
            for (var i = 0; i < SmoothTime; i++)
            {
                SmoothMap();
            }
            EraseSmallSeas();
            ProcessLands();
        }
        
        
        void EraseSmallSeas()
        {
            var flagMap = new FlagMap(WorldWidth, WorldHeight);
            foreach (var coord in WorldMap.IterateMap())
            {
                if (flagMap.GetFlag(coord) || (WorldMap.GetMap(coord) != 0)) continue;
                var newRegion = GetRegionCoords(coord);
                foreach (var regionCoord in newRegion)
                {
                    flagMap.SetFlag(regionCoord);
                }

                if (newRegion.Count >= SeaThresholdSize) continue;
                // Erase seas with small size.
                foreach (var regionCoord in newRegion)
                {
                    WorldMap.SetMap(regionCoord, 1);
                }
            }
        }

        void ProcessLands()
        {
            var flagMap = new FlagMap(WorldWidth, WorldHeight);
            foreach (var coord in WorldMap.IterateMap())
            {
                if (flagMap.GetFlag(coord) || (WorldMap.GetMap(coord) == 0)) continue;
                var newRegion = GetRegionCoords(coord);
                foreach (var regionCoord in newRegion)
                {
                    flagMap.SetFlag(regionCoord);
                }
                if (newRegion.Count < LandThresholdSize &&
                    Utils.ProcessRandom.Next(0, 100) < LandErasePercent)
                {
                    // Randomly erase lands with small size.
                    foreach (var regionCoord in newRegion)
                    {
                        WorldMap.SetMap(regionCoord, 0);
                    }
                    continue;
                }
                // Get the regions of the lands.
                _mainLands.Add(new MainLand(newRegion));
            }

            foreach (var mainLand in _mainLands)
            {
                foreach (var coord in mainLand.Coords)
                {
                    WorldMap.SetMap(coord, Utils.ProcessRandom.Next(1, BaseAreaPrefabs.Length));
                }
            }

        }

        List<WorldCoord> GetRegionCoords(WorldCoord startCoord)
        {
            // Get the regions of the tile around the given coord with same type of tiles.
            var tiles = new List<WorldCoord>();
            var flagMap = new FlagMap(WorldWidth, WorldHeight);
            var tileType = WorldMap.GetMap(startCoord);
            
            var queue = new Queue<WorldCoord>();
            queue.Enqueue(startCoord);
            flagMap.SetFlag(startCoord);

            while (queue.Count > 0)
            {
                var coord = queue.Dequeue();
                tiles.Add(coord);
                foreach (var neighbourCoord in coord.GetNeighbourCoords())
                {
                    if (flagMap.GetFlag(neighbourCoord) || WorldMap.GetMap(neighbourCoord) != tileType) 
                        continue;
                    flagMap.SetFlag(neighbourCoord);
                    queue.Enqueue(neighbourCoord);
                }
            }
            return tiles;
        }

        void SmoothMap()
        {
            foreach(var coord in WorldMap.IterateMap())
            {
                var neighbourLandTiles = GetSurroundingLandCount(coord);
                if (neighbourLandTiles > 0)
                {
                    WorldMap.SetMap(coord, 1);
                }
                else if (neighbourLandTiles < 0)
                {
                    WorldMap.SetMap(coord, 0);
                }
            }
        }

        int GetSurroundingLandCount(WorldCoord coord)
        {
            var landCount = 0;
            foreach (var neighbourCoord in coord.GetSurroundCoords())
            {
                if (WorldMap.GetMap(neighbourCoord) == 1)
                {
                    landCount += 1;
                }
                else
                {
                    landCount -= 1;
                }
            }

            return landCount;
        }

        private class FlagMap
        {
            private bool[,] _mapFlags;
            private readonly int _width;
            private readonly int _height;

            public FlagMap(int width, int height)
            {
                this._width = width;
                this._height = height;
                _mapFlags = new bool[width, height];
            }

            public bool GetFlag(WorldCoord coord)
            {
                return _mapFlags[coord.GetX(), coord.GetY()];
            }

            public void SetFlag(WorldCoord coord)
            {
                _mapFlags[coord.GetX(), coord.GetY()] = true;
            }

            public void ResetFlag(WorldCoord coord)
            {
                _mapFlags[coord.GetX(), coord.GetY()] = false;
            }

            public void ResetAll(WorldCoord coord)
            {
                _mapFlags = new bool[_width, _height];
            }
        }
    }
}