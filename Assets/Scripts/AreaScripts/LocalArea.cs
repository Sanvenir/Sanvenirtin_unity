using System.Collections.Generic;
using MapScripts;
using ObjectScripts;
using ObjectScripts.CharSubstance;
using ObjectScripts.RaceScripts;
using UnityEngine;
using UnityEngine.Tilemaps;
using UtilScripts;

namespace AreaScripts
{
    public abstract class LocalArea: MonoBehaviour
    {
        [HideInInspector]
        public int Identity;
        
        [HideInInspector]
        public bool OnWorld = true;
        
        [HideInInspector]
        public EarthMapCoord EarthMapCoord;
        
        [HideInInspector]
        public Vector2Int WorldStartCoord;
        
        [HideInInspector]
        public int LocalWidth, LocalHeight;
        
        public Tilemap Tilemap;

        public virtual void Initialize(int identity, Vector2Int startCoord)
        {
            Identity = identity;
            Tilemap = GetComponent<Tilemap>();
            if (!OnWorld) return;
            EarthMapCoord = EarthMapCoord.CreateFromIdentity(identity);
            LocalWidth = EarthMapManager.LocalWidth;
            LocalHeight = EarthMapManager.LocalHeight;
            WorldStartCoord = startCoord;
        }

        public bool IsWorldCoordInsideArea(Vector2Int coord)
        {
            return Tilemap.HasTile(Utils.Vector2IntTo3(coord));
        }

        public Vector2Int WorldCoordToLocal(Vector2Int coord)
        {
            return coord - WorldStartCoord;
        }
        
        protected IEnumerable<Vector3Int> IterateWorldCoordV3()
        {
            for (var x = 0; x != LocalWidth; x++)
            for (var y = 0; y != LocalHeight; y++)
            {
                yield return new Vector3Int(
                    x + WorldStartCoord.x, 
                    y + WorldStartCoord.y, 0);
            }
        }
        
        public IEnumerable<Vector2Int> IterateWorldCoord()
        {
            for (var x = 0; x != LocalWidth; x++)
            for (var y = 0; y != LocalHeight; y++)
            {
                yield return new Vector2Int(x, y) + WorldStartCoord;
            }
        }

        public Substance GenerateSubstance(Substance substancePrefab, Vector2Int worldCoord)
        {
            var pos = SceneManager.Instance.WorldCoordToPos(worldCoord);
            var instance = Instantiate(substancePrefab);
            instance.Initialize(worldCoord, Identity);
            if (instance.CheckCollider()) return instance;
            Destroy(instance.gameObject);
            return null;
        }

        public Character RandomGenerateRaceCharacters(Character raceCharacter, Vector2Int worldCoord)
        {
            var pos = SceneManager.Instance.WorldCoordToPos(worldCoord);
            var instance = Instantiate(raceCharacter);
            GameSetting.Instance.RaceList[instance.RaceIndex].RandomRefactor(instance);
            var character = instance.GetComponent<Character>();
            character.Initialize(worldCoord, Identity);
            if (character.CheckCollider()) return instance;
            Destroy(instance.gameObject);
            return null;
        }

        public BuildingTilemap GenerateBuilding(BuildingTilemap buildingTilemap, Vector2Int worldCoord)
        {
            var instance = Instantiate(
                buildingTilemap, SceneManager.Instance.Grid.transform);
            instance.Initialize(worldCoord, this);
            if (instance.CheckCollider()) return instance;
            Destroy(instance.gameObject);
            return null;
        }
    }
}