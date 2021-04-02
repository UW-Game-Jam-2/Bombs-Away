using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BombIndicator : MonoBehaviour
{

    public void updateBombIndicator(List<GameObject> bombs, int maxBombs)
    {
        for (int index = 0; index < maxBombs; index++)
        {
            Image bombIndicator = transform.GetChild(index).GetComponent<Image>();
            if (index < bombs.Count)
            {
                bombIndicator.sprite = bombs[index].GetComponent<Bomb>().bombRepresentation;
            } else
            {
                bombIndicator.sprite = null;
            }
        }
       
        
    }
}
