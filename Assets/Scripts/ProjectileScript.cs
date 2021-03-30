using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public float detonateTime = 1;
    public float radius;

    public LayerMask terrainLayer;

    public GameObject explosionFx;
    

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

        SpawnExplosionFX();
        DoCameraShake();

        Destroy(gameObject);
    }


    void SpawnExplosionFX()
    {
        GameObject explosion =  Instantiate(explosionFx, transform.position, Quaternion.identity);
        explosion.transform.localScale *= (radius + 1);
        Destroy(explosion, .2f);
    }

    void DoCameraShake()
    {
        Camera.main.GetComponent<CameraShake>().shakeDuration = 0.2f;

    }
}
