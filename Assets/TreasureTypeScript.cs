using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureTypeScript : MonoBehaviour
{
    [SerializeField] TreasureType type;
    [SerializeField] int baseAmount;
    [SerializeField] int variance;
    [SerializeField] CurrencyType currency;

    public int Amount()
    {
        return baseAmount + Random.Range(-variance, variance);
    }
}
