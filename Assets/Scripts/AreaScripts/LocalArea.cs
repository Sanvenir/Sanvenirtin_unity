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
        public WorldCoord WorldCoord;
        public Vector2Int GlobalStartCoord;
        public int LocalWidth, LocalHeight;
        
        public Tilemap Tilemap;

        public virtual void Initialize(int identity, Vector2Int startCoord)
        {
            Identity = identity;
            Tilemap = GetComponent<Tilemap>();
            if (!OnWorld) return;
            WorldCoord = WorldCoord.CreateFromIdentity(identity);
            LocalWidth = WorldManager.LocalWidth;
            LocalHeight = WorldManager.LocalHeight;
            GlobalStartCoord = startCoord;
        }

        public bool IsGlobalCoordInsideArea(Vector2Int coord)
        {
            return Tilemap.HasTile(Utils.Vector2IntTo3(coord));
        }

        public Vector2Int GlobalCoordToLocal(Vector2Int coord)
        {
            return coord - GlobalStartCoord;
        }
        
        protected IEnumerable<Vector3Int> IterateGlobalCoordV3()
        {
            for (var x = 0; x != LocalWidth; x++)
            for (var y = 0; y != LocalHeight; y++)
            {
                yield return new Vector3Int(
                    x + GlobalStartCoord.x, 
                    y + GlobalStartCoord.y, 0);
            }
        }
        
        public IEnumerable<Vector2Int> IterateGlobalCoord()
        {
            for (var x = 0; x != LocalWidth; x++)
            for (var y = 0; y != LocalHeight; y++)
            {
                yield return new Vector2Int(x, y) + GlobalStartCoord;
            }
        }

        public GameObject GenerateSubstance(GameObject substancePrefab, Vector2Int globalCoord)
        {
            var pos = SceneManager.Instance.GlobalCoordToPos(globalCoord);
            var instance = Instantiate(substancePrefab);
            var substance = instance.GetComponent<Substance>();
            if (Physics2D.OverlapPoint(pos, substance.BlockLayer) != null)
            {
                Destroy(instance);
                return null;
            }
            substance.Initialize(globalCoord, Identity);
            return instance;
        }
    }
}