using System.Collections.Generic;
using DefaultNamespace;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.Tilemaps;
using UtilScripts;

namespace AreaScripts
{
    public class RandomLocalArea: LocalArea
    {

        public TileBase GroundTile;
        public GameObject[] ObjectPrefabs;
        public float ObjectsGenerateRatio = 0.1f;

        public override void Initialize(int identity, Vector2Int startCoord)
        {
            base.Initialize(identity, startCoord);
            foreach (var coord in IterateGlobalCoordV3())
            {
                Tilemap.SetTile(coord, GroundTile);
            }

            foreach (var coord in IterateGlobalCoord())
            {
                
                if (!(Utils.ProcessRandom.NextDouble() < ObjectsGenerateRatio)) continue;
                var index = Utils.ProcessRandom.Next(ObjectPrefabs.Length);
                var instance = GenerateSubstance(ObjectPrefabs[index], coord);
            }
        }
    }
}