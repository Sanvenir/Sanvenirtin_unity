using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UtilScripts;

namespace MapScripts
{
//    [CustomEditor(typeof(EarthMapManager))]
//    public class EarthMapManagerEditor : Editor
//    {
//        public override void OnInspectorGUI()
//        {
//            base.OnInspectorGUI();
//            EarthMapManager.MapWidth = EditorGUILayout.IntField("Map Width", EarthMapManager.MapWidth);
//            EarthMapManager.MapHeight = EditorGUILayout.IntField("Map Height", EarthMapManager.MapHeight);
//            EarthMapManager.LocalWidth = EditorGUILayout.IntField("Local Width", EarthMapManager.LocalWidth);
//            EarthMapManager.LocalHeight = EditorGUILayout.IntField("Local Height", EarthMapManager.LocalHeight);
//        }
//    }
    public class EarthMapManager: MonoBehaviour
    {
        public static int MapWidth = 200, MapHeight = 100;
        public static int LocalWidth = 20, LocalHeight = 20;
        
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

        [HideInInspector]
        public EarthMap EarthMap;
        
        private readonly List<MainLand> _mainLands = new List<MainLand>();

        public void GenerateMap()
        {
            EarthMap = new EarthMap(MapWidth, MapHeight);
            MainLand.SetMap(EarthMap);

            foreach(var coord in EarthMap.IterateMap())
            {
                EarthMap.SetMap(coord, Utils.ProcessRandom.Next(0, 100) < LandPercent ? 1 : 0);
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
            var flagMap = new FlagMap(MapWidth, MapHeight);
            foreach (var coord in EarthMap.IterateMap())
            {
                if (flagMap.GetFlag(coord) || (EarthMap.GetMap(coord) != 0)) continue;
                var newRegion = GetRegionCoords(coord);
                foreach (var regionCoord in newRegion)
                {
                    flagMap.SetFlag(regionCoord);
                }

                if (newRegion.Count >= SeaThresholdSize) continue;
                // Erase seas with small size.
                foreach (var regionCoord in newRegion)
                {
                    EarthMap.SetMap(regionCoord, 1);
                }
            }
        }

        void ProcessLands()
        {
            var flagMap = new FlagMap(MapWidth, MapHeight);
            foreach (var coord in EarthMap.IterateMap())
            {
                if (flagMap.GetFlag(coord) || (EarthMap.GetMap(coord) == 0)) continue;
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
                        EarthMap.SetMap(regionCoord, 0);
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
                    EarthMap.SetMap(coord, Utils.ProcessRandom.Next(1, BaseAreaPrefabs.Length));
                }
            }

        }

        List<EarthMapCoord> GetRegionCoords(EarthMapCoord startCoord)
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

        void SmoothMap()
        {
            foreach(var coord in EarthMap.IterateMap())
            {
                var neighbourLandTiles = GetSurroundingLandCount(coord);
                if (neighbourLandTiles > 0)
                {
                    EarthMap.SetMap(coord, 1);
                }
                else if (neighbourLandTiles < 0)
                {
                    EarthMap.SetMap(coord, 0);
                }
            }
        }

        int GetSurroundingLandCount(EarthMapCoord coord)
        {
            var landCount = 0;
            foreach (var neighbourCoord in coord.GetSurroundCoords())
            {
                if (EarthMap.GetMap(neighbourCoord) == 1)
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