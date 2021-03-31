using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainDestroyer : MonoBehaviour
{

    // Tilemaps needed to draw/erase tiles from them map
    public Tilemap terrain;
    public Tilemap backgroundTerrain;

    // Tile that gets drawn in the background when tiles are destroyed
    public TileBase explodedTile;

    // Singleton instance
    public static TerrainDestroyer instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }

    }

    // Destroys terrain, called from the Bomb with specific position
    public void DestroyTerrain_Bomb(Vector3 translatedPosition) {
        Vector3Int tilePos = terrain.WorldToCell(translatedPosition);
        if (terrain.GetTile(tilePos) != null) {
            DestroyTile(tilePos);
        }
    }

    // Sets the tile in the foreground to nil and draws the background tile.
    void DestroyTile(Vector3Int tilePosition)
    {
        terrain.SetTile(tilePosition, null);
        backgroundTerrain.SetTile(tilePosition, explodedTile);
    }
}
