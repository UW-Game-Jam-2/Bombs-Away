using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using DG.Tweening;

// Listens for bomb explosions and checks to see if any loot has been uncovered
public class LootDetectorScript: MonoBehaviour
{

    public TileBase explodeTile;
    public Tilemap foregroundTilemap;
    public float percentToUncover = 0.75f;
    public CoinManagerScript coinManager;

    [Space]
    [Header ("Loot settings")]

    public Color alertColor = Color.red;
    public int flashes = 10;
    public float timeBetweenFlashes = 0.2f;

    [Header ("Treasure Settings")]
    public float timeBeforeTreasureDisappears = 0.5f;

    [Header("Ordinance Settings")]
    public float detonationTime = 0.5f;
    public TileBase debugTile;

    private Tilemap lootTilemap;
    private Dictionary<Vector3Int, List<Vector3Int>> lootToForegroundTiles;

    private void Start()
    {
        // grab the loot tile map
        lootTilemap = GetComponent<Tilemap>();

        // Create a map of Loot Tile coordinates to an array of Foreground tile coordinates.
        // This will be used later on to determine if a loot tile is some % uncovered.
        lootToForegroundTiles = new Dictionary<Vector3Int, List<Vector3Int>>();


        // Iterate over the foregorund cell by cell
        // For any foreground cell located over a loot tile, save this in a dictionary that maps loot tiles to an array of foreground cells
        // Because Loot tiles are scaled to be 4 times bigger than Foreground tiles, the value for each loot tile key entry can map to up a List of up to 16 foreground tiles
        for (int x = foregroundTilemap.cellBounds.min.x; x < foregroundTilemap.cellBounds.max.x; x++)
        {
            for (int y = foregroundTilemap.cellBounds.min.y; y < foregroundTilemap.cellBounds.max.y; y++)
            {
                Vector3Int foregroundTilePosition = new Vector3Int(x, y, 0);
                TileBase tile = foregroundTilemap.GetTile(foregroundTilePosition);

                Vector3 foregroundWorldPosition = foregroundTilemap.CellToWorld(foregroundTilePosition);

                Vector3Int lootTileLocation = lootTilemap.WorldToCell(foregroundWorldPosition);


                if (tile != null)
                {
                    TileBase lootTile = lootTilemap.GetTile(lootTileLocation);

                    if (lootTile != null)
                    {
                        // At this point there IS a foreground tile AND loot tile. Save this information for later!

                        // Create a list or grab the one that already exists
                        List<Vector3Int> listOfForegroundTiles = lootToForegroundTiles.ContainsKey(lootTileLocation) ? lootToForegroundTiles[lootTileLocation] : new List<Vector3Int>();

                        if (listOfForegroundTiles.Count == 0)
                        {
                            // There is NO list, so let's start saving one
                            List <Vector3Int> newList = new List<Vector3Int>();
                            newList.Add(foregroundTilePosition);
                            lootToForegroundTiles[lootTileLocation] = newList;

                        }
                        else
                        {
                            // There IS a list, so just append to it and add it back to the dictionary
                            listOfForegroundTiles.Add(foregroundTilePosition);
                            lootToForegroundTiles[lootTileLocation] = listOfForegroundTiles;
                        }
                    } 
                }
            }

        }
    }

    void OnEnable()
    {
        Bomb.detonate += CheckLocationIsUncovered;
    }

    void OnDisable()
    {
        Bomb.detonate -= CheckLocationIsUncovered;
    }

    void CheckLocationIsUncovered()
    {

        // keep track of the loot tiles that have NOT need collected
        var updatedLootToForegroundTiles = new Dictionary<Vector3Int, List<Vector3Int>>();

        foreach (KeyValuePair<Vector3Int, List<Vector3Int>> kvp in lootToForegroundTiles)
        {
            // grab some properties for the loot tile
            Vector3Int lootTileLocation = kvp.Key;
            Vector3 lootTileWorldLocation = lootTilemap.CellToWorld(lootTileLocation);
            TileBase lootTile = lootTilemap.GetTile(kvp.Key);

            // keep track of the destroyed count
            int destroyedCount = 0;
            int startingCount = kvp.Value.Count;
            foreach (Vector3Int foregroundTileLocation in kvp.Value)
            {
                TileBase foregroundTile = foregroundTilemap.GetTile(foregroundTileLocation);

                if (foregroundTile == null)
                {
                    destroyedCount++;
                } 
            }

            // Check to see if the destoryed percentage is greater than the percent needed to uncover the treasure
            if ((float)destroyedCount / (float)startingCount > percentToUncover)
            {
                // get the data regarding how much loot is in the treasure
                GameObject prefab = lootTilemap.GetInstantiatedObject(lootTileLocation);

                // grab the rendered from the treasure prefab to use to shift the sprite
                SpriteRenderer renderer = prefab.GetComponent<SpriteRenderer>();

                // shift the sprite up and over 1/2 the width and height (this accounts for the fact that the lootTileWorldLocation is at the bottom left of the tile
                Vector3 shiftedLootTileLocation = lootTileWorldLocation + new Vector3(x: renderer.bounds.size.x / 2, y: renderer.bounds.size.y / 2);

                // create the loot object using the provided prefab at the shifted location
                GameObject gameObject = Instantiate(prefab, shiftedLootTileLocation, Quaternion.identity);

                // remove the loot item from the loot map
                lootTilemap.SetTile(lootTileLocation, null);

                if (gameObject != null)
                {

                    // add the sprite to the screen
                    gameObject.GetComponent<SpriteRenderer>().enabled = true;
                    gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 1.0f);
                    gameObject.SetActive(true);
                    gameObject.transform.position = shiftedLootTileLocation;

                    TreasureTypeScript treasure;
                    gameObject.TryGetComponent<TreasureTypeScript>(out treasure);

                    Bomb bomb;
                    gameObject.TryGetComponent<Bomb>(out bomb);

                    if (treasure != null)
                    {
                        int treasureToCollect = treasure.Amount();
                        

                        // start a cortoutine that flickers the loot and then calls collect coins
                        StartCoroutine(TickLoot(gameObject, treasureToCollect, lootTileWorldLocation));
                        ObjectivesManagerScript.instance.UpdateChestCount();
                    }
                    else if (bomb != null)
                    {
                        bomb.GetComponent<Collider2D>().enabled = true;
                        bomb.detonateTime = detonationTime;
                    }
                    else
                    {
                        throw new ArgumentException("Treasure or bomb cannot be null", nameof(treasure));
                    }

                } else
                {
                    throw new ArgumentException("Game object cannot be null", nameof(gameObject));
                }
            }
            else
            {
                // otherwise add the key value entry into the new dictionary so that we can check these later on
                updatedLootToForegroundTiles[kvp.Key] = kvp.Value;
            }
        }

        // update the loot to foreground store so that we dont collect loot multiple times
        lootToForegroundTiles = updatedLootToForegroundTiles;
    }

    // Function to make the loot flicker then collect coins 
    IEnumerator TickLoot(GameObject loot, int coinsCollected, Vector3 worldLocation)
    {
        Color original = loot.GetComponent<SpriteRenderer>().color;

        /// flash a color
        for (int i = 0; i < flashes; i++)
        {
            Color currentColor = loot.GetComponent<SpriteRenderer>().color;
            if (currentColor == alertColor)
            {
                loot.GetComponent<SpriteRenderer>().color = Color.white;

            } else
            {
                loot.GetComponent<SpriteRenderer>().color = alertColor;
            }
            yield return new WaitForSeconds(timeBetweenFlashes);
        }

        // set it back
        loot.GetComponent<SpriteRenderer>().color = original;

        // destory the prefab
        Destroy(loot, timeBeforeTreasureDisappears);

        // tell the coin manager to do its thing.
        coinManager.CollectCoins(worldLocation, coinsCollected);


    }

}
