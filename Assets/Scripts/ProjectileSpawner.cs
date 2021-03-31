using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{

    public GameObject[] bombPrefabs;
    public ExplosionType explosionType;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            spawnPosition.z = 0;
            SpawnProjectileAtLocation(spawnPosition);

        }
    }

    // Spawns a bomb prefab
    void SpawnProjectileAtLocation(Vector3 spawnPosition)
    {
        if (explosionType.Equals(ExplosionType.BASIC)) {
            Instantiate(bombPrefabs[0], spawnPosition, Quaternion.identity);
        } else if (explosionType.Equals(ExplosionType.HORIZONTAL)) {
            Instantiate(bombPrefabs[1], spawnPosition, Quaternion.identity);
        } else if (explosionType.Equals(ExplosionType.VERTICAL)) {
            Instantiate(bombPrefabs[2], spawnPosition, Quaternion.identity);
        } else if (explosionType.Equals(ExplosionType.MOAB)) {
            Instantiate(bombPrefabs[3], spawnPosition, Quaternion.identity);
        }
    }
}
