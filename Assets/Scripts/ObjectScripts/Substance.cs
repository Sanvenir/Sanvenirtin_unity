using AreaScripts;
using ObjectScripts.BodyPartScripts;
using UnityEngine;
using UtilScripts;

namespace ObjectScripts
{
    public class Substance: ComplexObject
    {   
        // Read Only
        [HideInInspector]
        public int AreaIdentity;
        
        public void MoveTo(Vector2Int worldCoord)
        {
            WorldPos = SceneManager.Instance.WorldCoordToPos(worldCoord);
            SpriteRenderer.sortingOrder = -worldCoord.y;
            transform.position = WorldPos;
        }
        
        public override void Initialize(Vector2Int worldCoord, int areaIdentity)
        {
            base.Initialize(worldCoord, areaIdentity);
            AreaIdentity = areaIdentity;
            MoveTo(worldCoord);
        }


        protected override void LateUpdate()
        {
            base.LateUpdate();
            WorldPos = SceneManager.Instance.NormalizeWorldPos(WorldPos);
            if (!SceneManager.Instance.ActivateAreas.ContainsKey(AreaIdentity))
            {
                Destroy(gameObject);
                return;
            }

            var area = SceneManager.Instance.ActivateAreas[AreaIdentity];
            if (area.IsWorldCoordInsideArea(WorldCoord)) return;
            area = SceneManager.Instance.WorldPosToArea(WorldPos);
            if (area == null)
            {
                Destroy(gameObject);
                return;
            }
            AreaIdentity = area.Identity;
        }

        public bool CheckColliderAtWorldCoord(
            Vector2Int coord, out ComplexObject complexObject)
        {
            Collider2D.offset = coord - WorldCoord;
            var result = CheckCollider(out complexObject);
            Collider2D.offset = Vector2.zero;
            return result && SceneManager.Instance.GetTileType(coord) != TileType.Block;
        }

        public bool CheckColliderAtWorldCoord(Vector2Int coord)
        {
            Collider2D.offset = coord - WorldCoord;
            var result = CheckCollider();
            Collider2D.offset = Vector2.zero;
            return result && SceneManager.Instance.GetTileType(coord) != TileType.Block;
        }

        private static ContactFilter2D _blockFilter;
        private static bool _hasSetBlockFilter;

        private static ContactFilter2D BlockFilter
        {
            get
            {
                if (_hasSetBlockFilter) return _blockFilter;
                _blockFilter = new ContactFilter2D();
                _blockFilter.SetLayerMask(SceneManager.Instance.BlockLayer);
                _hasSetBlockFilter = true;
                return _blockFilter;
            }
        }

        public bool CheckCollider<T>(out T collide)
            where T : ComplexObject
        {
            var colliders = new Collider2D[1];
            Collider2D.OverlapCollider(BlockFilter, colliders);
            collide = colliders[0] == null
                ? null
                : colliders[0].GetComponent<T>();
            return colliders[0] == null;
        }
        
        public bool CheckCollider()
        {
            var colliders = new Collider2D[1];
            Collider2D.OverlapCollider(BlockFilter, colliders);
            return colliders[0] == null;
        }

    }
}