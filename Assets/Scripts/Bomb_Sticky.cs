using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb_Sticky : Bomb
{
    public Bomb_Sticky() {
        explosionType = ExplosionType.STICKY;
        explosionRadius = 0.5f;
        xExplosionDistance = explosionRadius;
        yExplosionDistance = explosionRadius;
        explosionImpactDistance = explosionRadius;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Destroy(rb);
    }
}
