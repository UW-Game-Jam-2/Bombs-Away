using System.Collections;
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

    [Header ("Loot settings")]
    
    public GameObject treasurePrefab;
    public int flashes = 10;
    public float timeBetweenFlashes = 0.2f;
    public Color alertColor = Color.red;
    public int coinsPerLootBase = 15;
    public float coinPerLootVariance = 5.0f;

    private Tilemap lootTilemap;
    private List<Vector3> tileWorldLocations;
    private float lootToForegroundScaleRatio;

    private void Start()
    {
        lootTilemap = GetComponent<Tilemap>();

        tileWorldLocations = new List<Vector3>();

        foreach (var pos in lootTilemap.cellBounds.allPositionsWithin)
        {
            Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
            Vector3 place = lootTilemap.CellToWorld(localPlace);
            if (lootTilemap.HasTile(localPlace))
            {
                tileWorldLocations.Add(place);
            }
        }

        // set up other cosntants
        lootToForegroundScaleRatio = lootTilemap.transform.localScale.x / foregroundTilemap.transform.localScale.x;

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
        foreach (var worldLocation in tileWorldLocations)
        {
            Vector3Int foregroundTile = foregroundTilemap.WorldToCell(worldLocation);
            Vector3Int lootTileLocation = lootTilemap.WorldToCell(worldLocation);
            if (foregroundTilemap.GetTile(foregroundTile) == null)
            {
                TileBase tile = lootTilemap.GetTile(lootTileLocation);
                if (tile != null)
                {
                    // remove the loot item from the loot map
                    lootTilemap.SetTile(lootTileLocation, null);
                    GameObject lootPrefab = Instantiate(treasurePrefab, worldLocation, Quaternion.identity);

                    // active the loot prefab
                    lootPrefab.SetActive(true);

                    // move the loot prefab to the correct location
                    float collectedCoins = coinsPerLootBase + Random.Range(-coinPerLootVariance, coinPerLootVariance);

                    StartCoroutine(TickLoot(lootPrefab, (int)collectedCoins, worldLocation));

                }
            }
        }
    }

    // Function to make the loot flicker then collect coins 
    IEnumerator TickLoot(GameObject loot, int coinsCollected, Vector3 worldLocation)
    {

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

        // destory the prefab
        Destroy(loot);

        // tell the coin manager to do its thing.
        coinManager.CollectCoins(worldLocation, coinsCollected);


    }

}
