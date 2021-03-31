using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    private WindIndicator windIndicator;
    private WindDirection[] windDirections = { WindDirection.LEFT,
        WindDirection.RIGHT };

    private WindDirection currentWindDirection;
    private int currentWindStrength;

    public int maxWindStrength;

    // Start is called before the first frame update
    void Start()
    {
        windIndicator = GetComponentInChildren<WindIndicator>();
        updateIndicator();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject bomb in getBombs())
        {
            applyForceToBomb(bomb);
        }
    }

    public void BombDetonationEvent()
    {
        updateIndicator();
    }

    private void updateIndicator()
    {
        refreshWindValues();
        windIndicator.UpdateWindIndicator(currentWindDirection, currentWindStrength);
    }

    private void refreshWindValues()
    {
        currentWindStrength = Random.Range(1, maxWindStrength + 1);
        currentWindDirection = windDirections[Random.Range(0, 2)];
    }

    private void applyForceToBomb(GameObject bomb)
    {
        Vector3 forceDirection;
        if (currentWindDirection == WindDirection.LEFT)
        {
            forceDirection = new Vector3(-1, 0);
        } else
        {
            forceDirection = new Vector3(1, 0);
        }
        bomb.GetComponent<Rigidbody2D>().AddForce(forceDirection * (float)currentWindStrength);
    }

    private GameObject[] getBombs()
    {
        return GameObject.FindGameObjectsWithTag("Bomb");
    }
}
