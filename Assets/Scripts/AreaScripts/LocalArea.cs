using System.Collections.Generic;
using MapScripts;
using ObjectScripts;
using ObjectScripts.CharSubstance;
using UnityEngine;
using UnityEngine.Tilemaps;
using UtilScripts;

namespace AreaScripts
{
    /// <inheritdoc />
    /// <summary>
    ///     The basic local area of the game. During game, if the player is on the Earth Map, a group of map will be loaded for
    ///     seamless world
    /// </summary>
    public abstract class LocalArea : TilemapTerrain
    {
        [HideInInspector] public int Identity;

        // The EarthMap coord if these area is on the Earth Map, or null
        [HideInInspector] public EarthMapCoord EarthMapCoord;
        [HideInInspector] public Vector2Int WorldStartCoord;
        [HideInInspector] public int LocalWidth, LocalHeight;
        
        /// <summary>
        ///     Initialize function need to be called after instantiate;
        /// </summary>
        /// <param name="identity">Identity of the area to generate</param>
        /// <param name="startCoord">The start coord of this area(coord of left-bottom tile)</param>
        public virtual void Initialize(int identity, Vector2Int startCoord)
        {
            base.Initialize();
            Identity = identity;
            Tilemap = GetComponent<Tilemap>();
            if (identity < 0) return; // If identity less than 0, means it is not on the Earth Map
            EarthMapCoord = EarthMapCoord.CreateFromIdentity(identity);
            LocalWidth = EarthMapManager.LocalWidth;
            LocalHeight = EarthMapManager.LocalHeight;
            WorldStartCoord = startCoord;
        }


        /// <summary>
        ///     Whether a coord is in the area
        /// </summary>
        /// <param name="coord">The world coord</param>
        /// <returns>True if the coord is in the area</returns>
        public bool IsWorldCoordInsideArea(Vector2Int coord)
        {
            return Tilemap.HasTile(Utils.Vector2IntTo3(coord));
        }

        /// <summary>
        ///     Convert a world coord to local coord
        /// </summary>
        /// <param name="coord">The world coord</param>
        /// <returns>The converted local coord</returns>
        public Vector2Int WorldCoordToLocal(Vector2Int coord)
        {
            return coord - WorldStartCoord;
        }

        /// <summary>
        ///     Iterate all coords inside the area
        /// </summary>
        /// <returns>iterate world coords(Vector 3) inside the area</returns>
        public IEnumerable<Vector3Int> IterateWorldCoordV3()
        {
            for (var x = 0; x != LocalWidth; x++)
            for (var y = 0; y != LocalHeight; y++)
                yield return new Vector3Int(x + WorldStartCoord.x, y + WorldStartCoord.y, 0);
        }

        /// <summary>
        ///     Iterate all coords inside the area
        /// </summary>
        /// <returns>iterate world coords(Vector 2) inside the area</returns>
        public IEnumerable<Vector2Int> IterateWorldCoord()
        {
            for (var x = 0; x != LocalWidth; x++)
            for (var y = 0; y != LocalHeight; y++)
                yield return new Vector2Int(x, y) + WorldStartCoord;
        }

        /// <summary>
        ///     Instantiate a substance prefab on the given coord
        /// </summary>
        /// <param name="substancePrefab"></param>
        /// <param name="worldCoord">The world coord</param>
        /// <returns>Instance of generated substance, or null if the coord is occupied</returns>
        public Substance GenerateSubstance(Substance substancePrefab, Vector2Int worldCoord)
        {
            var pos = SceneManager.Instance.WorldCoordToPos(worldCoord);
            var instance = Instantiate(substancePrefab);
            instance.Initialize(worldCoord, Identity);
            if (instance.CheckCollider()) return instance;
            Destroy(instance.gameObject);
            return null;
        }

        /// <summary>
        ///     Instantiate a character prefab and randomize by its race on the given coord
        /// </summary>
        /// <param name="raceCharacter">Generated character</param>
        /// <param name="worldCoord">The world coord</param>
        /// <returns>Instance of generated character, or null if the coord is occupied</returns>
        public Character RandomGenerateRaceCharacters(Character raceCharacter, Vector2Int worldCoord)
        {
            var pos = SceneManager.Instance.WorldCoordToPos(worldCoord);
            var instance = Instantiate(raceCharacter);
            SceneManager.Instance.RaceList[instance.RaceIndex].RandomRefactor(instance);
            var character = instance.GetComponent<Character>();
            character.Initialize(worldCoord, Identity);
            if (character.CheckCollider()) return instance;
            Destroy(instance.gameObject);
            return null;
        }

        /// <summary>
        ///     Instantiate a building prefab on the given coord
        /// </summary>
        /// <param name="buildingTilemap">Generated building</param>
        /// <param name="worldCoord">The world coord</param>
        /// <returns>
        ///     Instance of generated building, or null if the coord is occupied(Check by private
        ///     function of building)
        /// </returns>
        public BuildingTilemap GenerateBuilding(BuildingTilemap buildingTilemap, Vector2Int worldCoord)
        {
            var instance = Instantiate(buildingTilemap, SceneManager.Instance.Grid.transform);
            instance.Initialize(worldCoord, this);
            return instance;
        }
    }
}