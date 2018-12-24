using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UtilScripts;

namespace AreaScripts
{
    public class BuildingTilemap: MonoBehaviour
    {
        public TilemapRenderer OutsideTilemapRenderer;
        public Tilemap OutsideTilemap;
        public Tilemap InsideTilemap;
        public Collider2D BlockCollider;
        public ContactFilter2D ContactFilter;
        public int BuildingWidth;
        public int BuildingHeight;

        private void Awake()
        {
        }

        public void Initialize(Vector2 startPos)
        {
            OutsideTilemap.transform.position = startPos;
            OutsideTilemapRenderer.sortingOrder = -(int) startPos.y;
        }

        public bool CheckCollider()
        {
            // Return False if there are colliders in the block tiles
            var colliders = new Collider2D[1];
            return BlockCollider.OverlapCollider(ContactFilter, colliders) == 0;
            
        }

        private void Update()
        {
            if (SceneManager.Instance.PlayerObject == null)
            {
                return;
            }

            OutsideTilemapRenderer.enabled = !InsideTilemap.HasTile(
                InsideTilemap.WorldToCell(SceneManager.Instance.PlayerObject.WorldPos));
        }
    }
}