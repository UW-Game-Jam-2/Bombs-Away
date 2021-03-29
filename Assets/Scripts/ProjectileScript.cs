using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public float detonateTime = 1;
    public float radius = 3;

    public LayerMask terrainLayer;
    

    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        StartCoroutine(TickBomb());
    }

    IEnumerator TickBomb()
    {
        Invoke("Detonate", detonateTime);
        while (true)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(.1f);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(.1f);
        }
    }

    void Detonate()
    {
        TerrainDestroyer.instance.DestroyTerrain(transform.position, radius);

        Destroy(gameObject);
    }

}
