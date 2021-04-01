using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BombSelector : MonoBehaviour
{
    public GameObject[] bombPrefabs;
    private List<GameObject> currentBombs = new List<GameObject>();
    private BombIndicator bombIndicator;
    public int maxBombs;

    void Start()
    {
        bombIndicator = GetComponentInChildren<BombIndicator>();
        for (int index = 0; index < maxBombs; index++)
        {
            PopulateNextBomb();
        }
    }

    public GameObject GetNextBombAndPopulateNext()
    {
        GameObject nextBomb = DequeueBomb();
        PopulateNextBomb();
        return nextBomb;
    }

    private GameObject ChooseNextBomb()
    {
        return bombPrefabs[Random.Range(0, bombPrefabs.Length)];
    }

    private void PopulateNextBomb()
    {
        GameObject nextBomb = ChooseNextBomb();
        Bomb bomb = nextBomb.GetComponent<Bomb>();
        currentBombs.Add(nextBomb);
        bombIndicator.updateBombIndicator(currentBombs, maxBombs);
    }

    private GameObject DequeueBomb()
    {
        GameObject lastBomb = currentBombs[0];
        currentBombs.RemoveAt(0);
        bombIndicator.updateBombIndicator(currentBombs, maxBombs);
        return lastBomb;
    }

}
