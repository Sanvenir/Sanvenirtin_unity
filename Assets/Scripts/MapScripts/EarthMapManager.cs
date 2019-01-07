using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UtilScripts;
using Random = System.Random;

namespace MapScripts
{
    /// <inheritdoc />
    /// <summary>
    ///     Generate and manage the Earth Map.
    /// </summary>
    public class EarthMapManager : MonoBehaviour
    {
        public static int MapWidth = 200, MapHeight = 100;

        /// <summary>
        ///     Width and Height of local areas on the Earth Map
        /// </summary>
        public static int LocalWidth = 20, LocalHeight = 20;

        /// <summary>
        ///     Percent of land init ratio
        /// </summary>
        public int LandPercent = 50;

        /// <summary>
        ///     Threshold of the min size of Sea or Land
        /// </summary>
        public int SeaThresholdSize = 100;

        public int LandThresholdSize = 100;


        /// <summary>
        ///     Percent of the possibility of erasing lands with small size
        /// </summary>
        public int LandErasePercent = 70;


        /// <summary>
        ///     Time of smooth algorithms
        /// </summary>
        public int SmoothTime = 5;

        public GameObject[] BaseAreaPrefabs;

        public string RandomSeed = "Random";

        public static Random ProcessRandom;

        [HideInInspector] public EarthMap EarthMap;

        private readonly List<MainLand> _mainLands = new List<MainLand>();

        /// <summary>
        ///     Generate an Earth Map automatically
        /// </summary>
        public void GenerateMap()
        {
            if (RandomSeed.Equals("random")) RandomSeed = DateTime.Now.ToString(CultureInfo.CurrentCulture);

            ProcessRandom = new Random(RandomSeed.GetHashCode());

            EarthMap = new EarthMap(MapWidth, MapHeight);
            MainLand.SetMap(EarthMap);

            foreach (var coord in EarthMap.IterateMap())
                EarthMap.SetMap(coord, ProcessRandom.Next(0, 100) < LandPercent ? 1 : 0);

            //Smooth the Map
            for (var i = 0; i < SmoothTime; i++) SmoothMap();
            EraseSmallSeas();
            ProcessLands();
        }


        private void EraseSmallSeas()
        {
            var flagMap = new FlagMap(MapWidth, MapHeight);
            foreach (var coord in EarthMap.IterateMap())
            {
                if (flagMap.GetFlag(coord) || EarthMap.GetMap(coord) != 0) continue;
                var newRegion = GetRegionCoords(coord);
                foreach (var regionCoord in newRegion) flagMap.SetFlag(regionCoord);

                if (newRegion.Count >= SeaThresholdSize) continue;
                // Erase seas with small size.
                foreach (var regionCoord in newRegion) EarthMap.SetMap(regionCoord, 1);
            }
        }

        private void ProcessLands()
        {
            var flagMap = new FlagMap(MapWidth, MapHeight);
            foreach (var coord in EarthMap.IterateMap())
            {
                if (flagMap.GetFlag(coord) || EarthMap.GetMap(coord) == 0) continue;
                var newRegion = GetRegionCoords(coord);
                foreach (var regionCoord in newRegion) flagMap.SetFlag(regionCoord);
                if (newRegion.Count < LandThresholdSize &&
                    ProcessRandom.Next(0, 100) < LandErasePercent)
                {
                    // Randomly erase lands with small size.
                    foreach (var regionCoord in newRegion) EarthMap.SetMap(regionCoord, 0);
                    continue;
                }

                // Get the regions of the lands.
                _mainLands.Add(new MainLand(newRegion));
            }

            foreach (var mainLand in _mainLands)
            foreach (var coord in mainLand.Coords)
                EarthMap.SetMap(coord, ProcessRandom.Next(1, BaseAreaPrefabs.Length));
        }

        private List<EarthMapCoord> GetRegionCoords(EarthMapCoord startCoord)
        {
            // Get the regions of the tile around the given coord with same type of tiles.
            var tiles = new List<EarthMapCoord>();
            var flagMap = new FlagMap(MapWidth, MapHeight);
            var tileType = EarthMap.GetMap(startCoord);

            var queue = new Queue<EarthMapCoord>();
            queue.Enqueue(startCoord);
            flagMap.SetFlag(startCoord);

            while (queue.Count > 0)
            {
                var coord = queue.Dequeue();
                tiles.Add(coord);
                foreach (var neighbourCoord in coord.GetNeighbourCoords())
                {
                    if (flagMap.GetFlag(neighbourCoord) || EarthMap.GetMap(neighbourCoord) != tileType)
                        continue;
                    flagMap.SetFlag(neighbourCoord);
                    queue.Enqueue(neighbourCoord);
                }
            }

            return tiles;
        }

        private void SmoothMap()
        {
            foreach (var coord in EarthMap.IterateMap())
            {
                var neighbourLandTiles = GetSurroundingLandCount(coord);
                if (neighbourLandTiles > 0)
                    EarthMap.SetMap(coord, 1);
                else if (neighbourLandTiles < 0) EarthMap.SetMap(coord, 0);
            }
        }

        private int GetSurroundingLandCount(EarthMapCoord coord)
        {
            var landCount = 0;
            foreach (var neighbourCoord in coord.GetSurroundCoords())
                if (EarthMap.GetMap(neighbourCoord) == 1)
                    landCount += 1;
                else
                    landCount -= 1;

            return landCount;
        }

        private class FlagMap
        {
            private bool[,] _mapFlags;
            private readonly int _width;
            private readonly int _height;

            public FlagMap(int width, int height)
            {
                _width = width;
                _height = height;
                _mapFlags = new bool[width, height];
            }

            public bool GetFlag(EarthMapCoord coord)
            {
                return _mapFlags[coord.GetX(), coord.GetY()];
            }

            public void SetFlag(EarthMapCoord coord)
            {
                _mapFlags[coord.GetX(), coord.GetY()] = true;
            }

            public void ResetFlag(EarthMapCoord coord)
            {
                _mapFlags[coord.GetX(), coord.GetY()] = false;
            }

            public void ResetAll(EarthMapCoord coord)
            {
                _mapFlags = new bool[_width, _height];
            }
        }
    }
}