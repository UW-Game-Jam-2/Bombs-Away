using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb_Horizontal : Bomb
{
    public Bomb_Horizontal() {
        explosionType = ExplosionType.HORIZONTAL;
        explosionRadius = 0.5f;
        xExplosionDistance = explosionRadius * 2;
        yExplosionDistance = explosionRadius / 3;
        explosionImpactDistance = explosionRadius * 2;
    }
}
