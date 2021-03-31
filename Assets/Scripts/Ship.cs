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
            Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y - renderer.bounds.extents.y);
            projectileSpawner.SpawnProjectileAtLocation(spawnPosition);

        }
    }

    
}
