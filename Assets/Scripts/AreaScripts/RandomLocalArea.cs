using System;
using ObjectScripts;
using ObjectScripts.CharSubstance;
using UnityEngine;
using UnityEngine.Tilemaps;
using UtilScripts;

namespace AreaScripts
{
    /// <inheritdoc />
    /// <summary>
    ///     Randomly create a Local area
    /// </summary>
    public class RandomLocalArea : LocalArea
    {
        [Serializable]
        public struct RandomRace
        {
            public Character RaceCharacter;
            public float RaceAppearRatio;
        }

        public TileBase[] GroundTiles;
        public ComplexObject[] ComplexObjectPrefabs;
        public BuildingTilemap[] BuildingTilemaps;
        public RandomRace[] RaceSetting;

        public float ObjectsGenerateRatio = 0.1f;
        public float BuildingProbability = 0.5f;

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="startCoord"></param>
        public override void Initialize(int identity, Vector2Int startCoord)
        {
            base.Initialize(identity, startCoord);
            foreach (var coord in IterateWorldCoordV3())
                Tilemap.SetTile(
                    coord, GroundTiles[Utils.ProcessRandom.Next(GroundTiles.Length)]);

            foreach (var coord in IterateWorldCoord())
            {
                var check = (float) Utils.ProcessRandom.NextDouble();
                foreach (var race in RaceSetting)
                {
                    check -= race.RaceAppearRatio;
                    if (!(check < 0)) continue;
                    RandomGenerateRaceCharacters(race.RaceCharacter, coord);
                    break;
                }

                if (check < 0) continue;

                if (ComplexObjectPrefabs.Length == 0 ||
                    !(Utils.ProcessRandom.NextDouble() < ObjectsGenerateRatio)) continue;
                var index = Utils.ProcessRandom.Next(ComplexObjectPrefabs.Length);
                GenerateSubstance(ComplexObjectPrefabs[index], coord);
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