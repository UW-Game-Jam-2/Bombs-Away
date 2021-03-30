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

    // Caculates the distance from the explosion to each potential from that center
    // It then destroys any tile that is within the the radius of the explosion
    public void DestroyTerrain(Vector3 explosionLocation, float radius)
    {

        /// The step function moves up by a small step because that is the scale of the tile map and we need to check every tile for this to work
        for (float x = -radius; x < radius; x += 0.05f)
        {
            for (float y = -radius; y < radius; y += 0.05f)
            {

                if (Mathf.Pow(x, 2) + Mathf.Pow(y, 2) < Mathf.Pow(radius, 2))
                {
                    Vector3 translatedPosition = explosionLocation + new Vector3(x, y, 0);
                    Vector3Int tilePos = terrain.WorldToCell(translatedPosition);
                    if (terrain.GetTile(tilePos) != null)
                    {
                        DestroyTile(tilePos);
                    }
                }
            }
        }

    }

    // Sets the tile in the foreground to nil and draws the background tile.
    void DestroyTile(Vector3Int tilePosition)
    {
        terrain.SetTile(tilePosition, null);
        backgroundTerrain.SetTile(tilePosition, explodedTile);
    }
}
