using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Ship : MonoBehaviour
{

    public float moveSpeed = 10;
    public ProjectileSpawner projectileSpawner;
    private float maxX = float.PositiveInfinity;
    private float minX = float.NegativeInfinity;

    private float attackSpeed = 2f;
    private float cooldown;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] gameObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in gameObjects)
        {
            if (obj.CompareTag("RightBoundary"))
            {
                maxX = obj.transform.position.x;
            }
            else if (obj.CompareTag("LeftBoundary"))
            {
                minX = obj.transform.position.x;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        if (horizontalInput > 0) {
            transform.localScale = new Vector3(-Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        } else if (horizontalInput < 0) {
            transform.localScale = new Vector3(Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        if (transform.position.x >= maxX && horizontalInput > 0)
        {
            // dont move
        }
        else if (transform.position.x <= minX && horizontalInput < 0)
        {
            //dont move
        } else {
            // transform.Translate(new Vector3(horizontalInput, 0, 0) * moveSpeed * Time.deltaTime);
            float xPos = transform.position.x;
            xPos += horizontalInput * moveSpeed * Time.deltaTime;
            transform.position = new Vector3(xPos, transform.position.y, transform.position.z) ;

        }

        // Throw a bomb if the user pressed space
        if (Input.GetButtonDown("FireBomb") && Time.time > cooldown)
        {
            Renderer renderer = GetComponent<Renderer>();
            // The new spawn position is y minus the space that the ship occupies
            // so that the projectiles start from the bottom of the ship.
            Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y - renderer.bounds.extents.y);

            projectileSpawner.SpawnProjectileAtLocation(spawnPosition, GetNextBomb());
            cooldown = Time.time + attackSpeed;
            ObjectivesManagerScript.instance.UpdateShotCount();

        }
    }

    private GameObject GetNextBomb()
    {
        GameObject bombObject = GameObject.FindGameObjectsWithTag("Bomb Selection")[0];
        return bombObject.GetComponent<BombSelector>().GetNextBombAndPopulateNext();
    }
}
