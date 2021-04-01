using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// Listens for bomb explosions and checks to see if any loot has been uncovered
public class LootDetectorScript : MonoBehaviour
{

    public Tilemap foregroundTilemap;
    public float percentToUncover = 0.75f;
    public Animation lootExplosion;

    private Tilemap lootTilemap;
    private List<Vector3> tileWorldLocations;
    private float lootToForegroundScaleRatio;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        lootTilemap = GetComponent<Tilemap>();

        tileWorldLocations = new List<Vector3>();

        foreach (var pos in lootTilemap.cellBounds.allPositionsWithin)
        {
            Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
            Vector3 place = lootTilemap.CellToWorld(localPlace);
            if (lootTilemap.HasTile(localPlace))
            {
                tileWorldLocations.Add(place);
                print(place);
            }
        }

        // set up other cosntants
        lootToForegroundScaleRatio = lootTilemap.transform.localScale.x / foregroundTilemap.transform.localScale.x;
    }

    void OnEnable()
    {
        Bomb.detonate += CheckForLoot;
    }

    private void OnDisable()
    {
        Bomb.detonate -= CheckForLoot;
    }

    void CheckForLoot()
    {
        print("checking");
        CheckLocationIsUncovered();

    }

    void CheckLocationIsUncovered()
    {
        foreach (var worldLocation in tileWorldLocations)
        {
            Vector3Int foregroundTile = foregroundTilemap.WorldToCell(worldLocation);
            Vector3Int lootTileLocation = lootTilemap.WorldToCell(worldLocation);
            if (foregroundTilemap.GetTile(foregroundTile) == null && lootTilemap.GetTile(lootTileLocation) != null)
            {
                print("im free");
                lootExplosion.transform.position = worldLocation;
                lootExplosion.Play();
                lootTilemap.SetTile(lootTileLocation, null);
            }
        }
    }
}
