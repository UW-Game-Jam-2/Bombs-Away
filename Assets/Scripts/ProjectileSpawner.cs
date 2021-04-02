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
            SpawnProjectileAtLocation(spawnPosition, RandomizeBomb());

        }
    }

    // Spawns a bomb prefab
    public void SpawnProjectileAtLocation(Vector3 spawnPosition, GameObject bomb)
    {
        Instantiate(bomb, spawnPosition, Quaternion.identity);
    }

    private GameObject RandomizeBomb()
    {
        return bombPrefabs[Random.Range(0, bombPrefabs.Length)];
    }
}
