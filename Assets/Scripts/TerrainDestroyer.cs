using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainDestroyer : MonoBehaviour
{

    public Tilemap terrain;
    //public Tilemap destoryTerrain;
    public TileBase explodedTile;

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

    public void DestroyTerrain(Vector3 explosionLocation, float radius)
    {

        /// The step function moves up by 0.25 because that is the scale of the tile map and we need to check every tile for this to work
        for (float x = -radius; x < radius; x += 0.25f)
        {
            for (float y = -radius; y < radius; y += 0.25f)
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

    void DestroyTile(Vector3Int tilePosition)
    {
        terrain.SetTile(tilePosition, null);
        //destoryTerrain.SetTile(tilePosition, explodedTile);
    }
}
