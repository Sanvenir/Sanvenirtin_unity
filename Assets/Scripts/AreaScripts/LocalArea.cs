using System.Collections.Generic;
using DefaultNamespace;
using ObjectScripts;
using UnityEngine;
using UnityEngine.Tilemaps;
using UtilScripts;

namespace AreaScripts
{
    public abstract class LocalArea: MonoBehaviour
    {
        public int Identity;
        public bool OnWorld = true;
        public EarthMapCoord EarthMapCoord;
        public Vector2Int WorldStartCoord;
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

        public GameObject GenerateSubstance(GameObject substancePrefab, Vector2Int worldCoord)
        {
            var pos = SceneManager.Instance.WorldCoordToPos(worldCoord);
            var instance = Instantiate(substancePrefab);
            var substance = instance.GetComponent<Substance>();
            substance.Initialize(worldCoord, Identity);
            if (substance.CheckCollider<Substance>() == null) return instance;
            Destroy(instance);
            return null;
        }
    }
}