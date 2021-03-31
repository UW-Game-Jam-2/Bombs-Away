using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb_Vertical : Bomb
{
    public Bomb_Vertical() {
        explosionType = ExplosionType.VERTICAL;
        explosionRadius = 0.5f;
        xExplosionDistance = explosionRadius / 3;
        yExplosionDistance = explosionRadius * 2;
        explosionImpactDistance = explosionRadius * 2;
    }
}
