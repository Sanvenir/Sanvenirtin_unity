using UnityEngine;
using UnityEngine.Tilemaps;

namespace AreaScripts
{
    /// <inheritdoc />
    /// <summary>
    ///     Make up a building, contains ground tiles, block tiles and outside tiles; when player enters the ground
    ///     tiles, the outside tiles will hide for player to inspect;
    /// </summary>
    public class BuildingTilemap : TilemapTerrain
    {
        public TilemapRenderer OutsideTilemapRenderer;
        public Collider2D BlockCollider;
        public ContactFilter2D ContactFilter;

        [HideInInspector] public LocalArea Area;
        public int BuildingWidth;
        public int BuildingHeight;

        /// <summary>
        ///     Initialize function need to be called after instantiate;
        /// </summary>
        /// <param name="startPos">The position of anchor of the building</param>
        /// <param name="area">Which area the building belongs to</param>
        public void Initialize(Vector2 startPos, LocalArea area)
        {
            base.Initialize();
            transform.position = startPos;
            OutsideTilemapRenderer.sortingOrder = -(int) startPos.y;
            Area = area;
            if (CheckCollider()) Destroy(gameObject);
            foreach (var coord in Tilemap.cellBounds.allPositionsWithin)
            {
                Tilemap.SetTileFlags(coord, TileFlags.None);
                Tilemap.SetColor(coord, Color.black);
            }
        }

        private bool CheckCollider()
        {
            // Return False if there are colliders in the block tiles
            var hits = new Collider2D[1];
            return BlockCollider.OverlapCollider(ContactFilter, hits) != 0;
        }

        private void Update()
        {
            if (SceneManager.Instance.PlayerObject == null) return;

            OutsideTilemapRenderer.enabled = !(
                HasTile(SceneManager.Instance.PlayerObject.WorldCoord) ||
                HasTile(SceneManager.Instance.SceneControlButton.SceneCursor.transform.position));
        }

        private void LateUpdate()
        {
            if (Area == null) Destroy(gameObject);
        }
    }
}