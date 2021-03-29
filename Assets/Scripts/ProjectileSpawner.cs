using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{

    public GameObject bombPrefab;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            spawnPosition.z = 1;
            SpawnProjectileAtLocation(spawnPosition);

        }
    }

    void SpawnProjectileAtLocation(Vector3 spawnPosition)
    {
        Instantiate(bombPrefab, spawnPosition, Quaternion.identity);
    }
}
