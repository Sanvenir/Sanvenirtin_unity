using UnityEngine;
using UnityEngine.Tilemaps;

namespace AreaScripts
{
    public class BlockTile: MonoBehaviour
    {
        public TilemapRenderer TilemapRenderer;
        public Tilemap Tilemap;

        private void Awake()
        {
            Tilemap = GetComponent<Tilemap>();
            TilemapRenderer = GetComponent<TilemapRenderer>();
        }

        public void Initialize(Vector2 startPos)
        {
            Tilemap.transform.position = startPos;
        }
    }
}