using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb_MOAB : Bomb
{
    public Bomb_MOAB() {
        explosionType = ExplosionType.MOAB;
        explosionRadius = 1f;
        xExplosionDistance = explosionRadius;
        yExplosionDistance = explosionRadius;
        explosionImpactDistance = explosionRadius;
    }
}
