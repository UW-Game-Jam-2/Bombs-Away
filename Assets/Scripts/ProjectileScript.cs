using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script to control the projectiles once they are spawned.
public class ProjectileScript : MonoBehaviour
{
    public float detonateTime = 1;
    public float radius;
    public float cameraShakeDuration = 0.2f;
    public float explosionDuration = 0.2f;

    public ExplosionType explosionType = ExplosionType.BASIC;

    public GameObject explosionFx;

    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        StartCoroutine(TickBomb());
    }

    // Function to make the bomb flicker white and red 
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

    // Makes calls to explode the bomb, spawn an explosion and shake the camera.
    void Detonate()
    {

        SpawnExplosionFX();
        DoCameraShake();



        Destroy(gameObject);
    }

    // Instantiates and plays the explosion animation.  Destroyed after 
    void SpawnExplosionFX()
    {
        GameObject explosion =  Instantiate(explosionFx, transform.position, Quaternion.identity);
        explosion.transform.localScale *= (radius + 1);
        Destroy(explosion, explosionDuration);
    }

    // Calls the camera to shake
    void DoCameraShake()
    {
        Camera.main.GetComponent<CameraShake>().shakeDuration = cameraShakeDuration;

    }
}
