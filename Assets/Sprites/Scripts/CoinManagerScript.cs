using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CoinManagerScript : MonoBehaviour
{

    [SerializeField] GameObject coinPrefab;
    [SerializeField] Transform coinTarget;
    [SerializeField] Text textField;

    [Space]
    [Header("Available coins: (coins to pool)")]
    [SerializeField] int maxCoins;

    [Space]
    [Header("Animation settings:")]
    [SerializeField] [Range(0.5f, 2.9f)] float minAnimationDuration;
    [SerializeField] [Range(2.9f, 0.5f)] float maxAnimationDuration;
    [SerializeField] Ease easeType;
    [SerializeField] float spread;

    private Queue<GameObject> coinsQueue = new Queue<GameObject>();
    private Vector3 targetPosition;
    private int _c = 0;

    public int Coins
    {
        get { return _c; }
        set
        {
            _c = value;

            // set the text value
            textField.text = $"x{_c}";
        }
    }

    void Awake()
    {
        targetPosition = coinTarget.position;

        //prepare pool
        PrepareCoins();

    }

    void PrepareCoins()
    {
        GameObject coin;
        for (int i = 0; i < maxCoins; i++)
        {
            coin = Instantiate(coinPrefab);
            coin.transform.parent = transform;
            coin.SetActive(false);
            coinsQueue.Enqueue(coin);
        }
    }

    void Animate(Vector3 collectPosition, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            // check for coin in queue
            if (coinsQueue.Count > 0)
            {
                // extract coin from queue
                GameObject coin = coinsQueue.Dequeue();

                // activate it to show it
                coin.SetActive(true);

                // set it's position to the collected loot's position
                coin.transform.position = collectPosition + new Vector3(Random.Range(-spread, spread), 0f, 0f);

                // animate it to the target position
                float duration = Random.Range(minAnimationDuration, maxAnimationDuration);
                coin.transform.DOMove(targetPosition, duration)
                    .SetEase(easeType)
                    .OnComplete( () =>
                    {
                        // hide coin from screen
                        coin.SetActive(false);

                        // add it back to the pool
                        coinsQueue.Enqueue(coin);

                        // increment the coins count
                        Coins++;
                    });

            }

        }
        
    }

    // Update is called once per frame
    public void CollectCoins(Vector3 collectPosition, int amount)
    {
        ObjectivesManagerScript.instance.UpdateCoinCount(amount);
        Animate(collectPosition, amount);
    }
}
