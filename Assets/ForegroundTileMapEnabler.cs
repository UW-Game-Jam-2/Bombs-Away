using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ForegroundTileMapEnabler : MonoBehaviour
{
    void OnEnable()
    {
        GetComponent<TilemapRenderer>().enabled = true;
    }
}
