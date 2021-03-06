using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class Bomb: MonoBehaviour
{
    // projectile script values
    public float detonateTime = 1;
    public float cameraShakeDuration = 0.2f;
    public float explosionDuration = 0.2f;
    public float explosionXMultiplier = 1.0f;
    public float explosionYMultiplier = 1.0f;
    public GameEvent explodeBombEvent;

    // Event that is triggered when the bomb explodes
    public static event Action detonate; 

    public ExplosionType explosionType { get; set; }
    public float explosionRadius { get; set; }
    public float xExplosionDistance { get; set; }
    public float yExplosionDistance { get; set; }
    public float explosionImpactDistance { get; set; }

    public GameObject explosionFx;
    public Sprite bombRepresentation;
    SpriteRenderer spriteRenderer;

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();


        StartCoroutine(TickBomb());
    }

    // Function to make the bomb flicker white and red 
    IEnumerator TickBomb() {
        Invoke("Detonate", detonateTime);
        while (true) {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(.1f);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(.1f);
        }
    }

    // Makes calls to explode the bomb, spawn an explosion and shake the camera.
    protected virtual void Detonate() {
        DestroyTerrain();

        explodeBombEvent.Raise();

        // this call must happen after `DestroyTerrain()` 
        detonate?.Invoke();

        SpawnExplosionFX();
        DoCameraShake();

        Destroy(gameObject);
    }

    // Instantiates and plays the explosion animation.  Destroyed after 
    protected void SpawnExplosionFX() {
        GameObject explosion = Instantiate(explosionFx, transform.position, Quaternion.identity);
        explosion.transform.localScale *= (transform.localScale.x + (explosionRadius + 1));
        Destroy(explosion, explosionDuration);
    }

    // Calls the camera to shake
    protected void DoCameraShake() {
        Camera.main.GetComponent<CameraShake>().shakeDuration = cameraShakeDuration;

    }

    public void DestroyTerrain() {
        float modifiedExplosionXDistance = explosionXMultiplier * xExplosionDistance;
        float modifiedExplosionYDistance = explosionYMultiplier * yExplosionDistance;

        List<Vector3> positionToDestory = new List<Vector3>();

        for (float x = -modifiedExplosionXDistance; x < modifiedExplosionXDistance; x += 0.05f) {
            for (float y = -modifiedExplosionYDistance; y < modifiedExplosionYDistance; y += 0.05f) {

                // Check the X axis to see if this tile falls in the modified X direction
                if (Mathf.Pow(x, 2) + Mathf.Pow(y, 2) < Mathf.Pow(explosionImpactDistance*modifiedExplosionXDistance , 2)) {
                    Vector3 translatedPosition = transform.position + new Vector3(x, y, 0);
                    positionToDestory.Add(translatedPosition);
                    //TerrainDestroyer.instance.DestroyTerrain_Bomb(translatedPosition);
                }


                // Check the Y axis to see if this tile falls in the modified Y direction
                if (Mathf.Pow(x, 2) + Mathf.Pow(y, 2) < Mathf.Pow(explosionImpactDistance * modifiedExplosionYDistance, 2))
                {
                    Vector3 translatedPosition = transform.position + new Vector3(x, y, 0);
                    positionToDestory.Add(translatedPosition);
                    //TerrainDestroyer.instance.DestroyTerrain_Bomb(translatedPosition);
                }
            }
        }

        // pass in all the affected positions and the source of the explosion
        TerrainDestroyer.instance.DestoryTerrain_Positions(positionToDestory, transform.position);// + new Vector3(spriteRenderer.bounds.size.x / 2, spriteRenderer.bounds.size.y / 2));
    }

 
}
