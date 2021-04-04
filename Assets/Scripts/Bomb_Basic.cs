using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb_Basic : Bomb
{
    public Bomb_Basic() {
        explosionType = ExplosionType.BASIC;
        explosionRadius = 0.5f;
        xExplosionDistance = explosionRadius;
        yExplosionDistance = explosionRadius;
        explosionImpactDistance = explosionRadius;
    }
}
