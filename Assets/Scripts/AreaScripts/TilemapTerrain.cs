using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace AreaScripts
{
    /// <inheritdoc />
    /// <summary>
    ///     A class for tilemap with TileType on each tile
    /// </summary>
    public class TilemapTerrain : MonoBehaviour
    {
        [HideInInspector] public Tilemap Tilemap;

        [HideInInspector] public TilemapRenderer TilemapRenderer;
        public Dictionary<Vector2Int, TileType> TileTypes = new Dictionary<Vector2Int, TileType>();

        public void SetTile(Vector2Int coord, TileBase tile, TileType tileType = TileType.None)
        {
            var cell = Tilemap.WorldToCell(SceneManager.Instance.WorldCoordToPos(coord));
            Tilemap.SetTile(cell, tile);
            Tilemap.SetTileFlags(cell, TileFlags.None);
            Tilemap.SetColor(cell, Color.black);
            TileTypes[coord] = tileType;
        }

        public void Initialize()
        {
            Tilemap = GetComponent<Tilemap>();
            TilemapRenderer = GetComponent<TilemapRenderer>();
        }

        public bool HasTile(Vector2Int coord)
        {
            return Tilemap.HasTile(Tilemap.WorldToCell(SceneManager.Instance.WorldCoordToPos(coord)));
        }

        public bool HasTile(Vector2 pos)
        {
            return Tilemap.HasTile(Tilemap.WorldToCell(pos));
        }

        public TileType GetTileType(Vector2Int coord)
        {
            return TileTypes.ContainsKey(coord) ? TileTypes[coord] : TileType.None;
        }
    }
}