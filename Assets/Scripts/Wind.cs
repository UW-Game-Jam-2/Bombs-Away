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
        UpdateIndicator();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject bomb in getBombs())
        {
            ApplyForceToBomb(bomb);
        }
    }

    public void BombDetonationEvent()
    {
        UpdateIndicator();
    }

    private void UpdateIndicator()
    {
        RefreshWindValues();
        windIndicator.UpdateWindIndicator(currentWindDirection, currentWindStrength);
    }

    private void RefreshWindValues()
    {
        currentWindStrength = Random.Range(0, maxWindStrength + 1);
        currentWindDirection = windDirections[Random.Range(0, 2)];
    }

    private void ApplyForceToBomb(GameObject bomb)
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
