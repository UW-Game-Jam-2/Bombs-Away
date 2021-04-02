using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{

    public float moveSpeed = 10;
    public ProjectileSpawner projectileSpawner;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(new Vector3(horizontalInput, 0, 0) * moveSpeed * Time.deltaTime);

        // Throw a bomb if the user pressed space
        if (Input.GetButtonDown("FireBomb"))
        {
            Renderer renderer = GetComponent<Renderer>();
            // The new spawn position is y minus the space that the ship occupies
            // so that the projectiles start from the bottom of the ship.
            Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y - renderer.bounds.extents.y);
            
            projectileSpawner.SpawnProjectileAtLocation(spawnPosition, GetNextBomb());

        }
    }

    private GameObject GetNextBomb()
    {
        GameObject bombObject = GameObject.FindGameObjectsWithTag("Bomb Selection")[0];
        return bombObject.GetComponent<BombSelector>().GetNextBombAndPopulateNext();
    }

    
}
