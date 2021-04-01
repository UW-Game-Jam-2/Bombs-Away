using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb_Cluster : Bomb
{
    public GameObject bombToSpawn;
    public Bomb_Cluster() {
        explosionType = ExplosionType.CLUSTER;
        explosionRadius = 0.5f;
        xExplosionDistance = explosionRadius;
        yExplosionDistance = explosionRadius;
        explosionImpactDistance = explosionRadius;
    }

    protected override void Detonate() {

        Vector3 currentBombPos = transform.position;

        Vector3 clusterPosition1 = new Vector3(currentBombPos.x + 0.5f, currentBombPos.y + 0.5f);
        Vector3 clusterPosition2 = new Vector3(currentBombPos.x, currentBombPos.y + 0.5f);
        Vector3 clusterPosition3 = new Vector3(currentBombPos.x - 0.5f, currentBombPos.y + 0.5f);

        GameObject bomb1 = Instantiate(bombToSpawn, clusterPosition1, Quaternion.identity);
        GameObject bomb2 = Instantiate(bombToSpawn, clusterPosition2, Quaternion.identity);
        GameObject bomb3 = Instantiate(bombToSpawn, clusterPosition3, Quaternion.identity);

        Rigidbody2D bomb1_rb = bomb1.GetComponent<Rigidbody2D>();
        Rigidbody2D bomb2_rb = bomb2.GetComponent<Rigidbody2D>();
        Rigidbody2D bomb3_rb = bomb3.GetComponent<Rigidbody2D>();

        bomb1_rb.velocity = new Vector2(5.0f, 3.0f);
        bomb2_rb.velocity = new Vector2(0f, 3.0f);
        bomb3_rb.velocity = new Vector2(-5.0f, 3.0f);

        DestroyTerrain();

        explodeBombEvent.Raise();

        SpawnExplosionFX();
        DoCameraShake();

        Destroy(gameObject);
    }
}
