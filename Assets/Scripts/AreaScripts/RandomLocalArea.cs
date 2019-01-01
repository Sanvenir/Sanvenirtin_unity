using System.Net;
using ObjectScripts;
using ObjectScripts.RaceScripts;
using UnityEngine;
using UnityEngine.Tilemaps;
using UtilScripts;

namespace AreaScripts
{
    public class RandomLocalArea : LocalArea
    {
        public TileBase[] GroundTiles;
        public Substance[] SubstancePrefabs;
        public BuildingTilemap[] BuildingTilemaps;
        public BasicRace[] Races;
        
        public float ObjectsGenerateRatio = 0.1f;
        public float BuildingProbability = 0.5f;

        public override void Initialize(int identity, Vector2Int startCoord)
        {
            base.Initialize(identity, startCoord);
            foreach (var coord in IterateWorldCoordV3())
            {
                Tilemap.SetTile(
                    coord, GroundTiles[Utils.ProcessRandom.Next(GroundTiles.Length)]);
            }

            foreach (var coord in IterateWorldCoord())
            {

                var check = (float)Utils.ProcessRandom.NextDouble();
                foreach (var race in Races)
                {
                    check -= race.AppearRatio;
                    if (!(check < 0)) continue;
                    GenerateRace(race, coord);
                    break;
                }

                if (check < 0) continue;
                
                if (SubstancePrefabs.Length == 0 ||
                    !(Utils.ProcessRandom.NextDouble() < ObjectsGenerateRatio)) continue;
                var index = Utils.ProcessRandom.Next(SubstancePrefabs.Length);
                GenerateSubstance(SubstancePrefabs[index], coord);
            }

            if (BuildingTilemaps.Length == 0 ||
                !(Utils.ProcessRandom.NextDouble() < BuildingProbability)) return;
            var building = BuildingTilemaps[
                Utils.ProcessRandom.Next(BuildingTilemaps.Length)];
            var buildingCoord = new Vector2Int(
                Utils.ProcessRandom.Next(LocalWidth - building.BuildingWidth),
                Utils.ProcessRandom.Next(LocalHeight - building.BuildingHeight)
            );
            
            GenerateBuilding(building, startCoord + buildingCoord);
        }
    }
}