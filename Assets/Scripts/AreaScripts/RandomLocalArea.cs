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
        public GameObject[] SubstancePrefabs;
        public float ObjectsGenerateRatio = 0.1f;

        public override void Initialize(int identity, Vector2Int startCoord)
        {
            base.Initialize(identity, startCoord);
            foreach (var coord in IterateWorldCoordV3())
            {
                Tilemap.SetTile(coord, GroundTile);
            }

            foreach (var coord in IterateWorldCoord())
            {
                
                if (!(Utils.ProcessRandom.NextDouble() < ObjectsGenerateRatio)) continue;
                var index = Utils.ProcessRandom.Next(SubstancePrefabs.Length);
                var instance = GenerateSubstance(SubstancePrefabs[index], coord);
            }
        }
    }
}