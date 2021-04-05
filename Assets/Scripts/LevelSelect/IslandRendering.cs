using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class IslandRendering : MonoBehaviour
{
    [SerializeField] List<Tilemap> tilemaps;
    [SerializeField] List<Collider2D> colliders;


    private void Start()
    {
        int highestLevelToBeSeen = GameManager.sharedInstance.playerInfo.highestLevelBeat;

        for (int i = 0; i < tilemaps.Count; i++)
        {
            // if you beat 1 then you see 2 and so on.  
            if (i <= highestLevelToBeSeen)
            {
                tilemaps[i].GetComponent<TilemapRenderer>().enabled = true;
                colliders[i].enabled = true;
            } else
            {
                tilemaps[i].GetComponent<TilemapRenderer>().enabled = false;
                colliders[i].enabled = false;
            }
        }
    }
}
