using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BackgroundTerrainBulider : MonoBehaviour
{

    // Tilemaps needed to draw/erase tiles from them map
    public Tilemap foregroundMap;
    public Tilemap backgroundMap;

    // Tile that gets drawn in the background when tiles are destroyed
    public TileBase backgroundTile;

    // Start is called before the first frame update
    void Start()
    {
        for (int x = foregroundMap.cellBounds.min.x; x < foregroundMap.cellBounds.max.x; x++)
        {
            for (int y = foregroundMap.cellBounds.min.y; y < foregroundMap.cellBounds.max.y; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                TileBase tile = foregroundMap.GetTile(tilePosition);
                if (tile != null)
                {
                    backgroundMap.SetTile(tilePosition, backgroundTile);
                }
            }

        }
 
    }
}
