using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindIndicator : MonoBehaviour
{

    public GameObject leftWindDirection;
    public GameObject rightWindDirection;

    public void UpdateWindIndicator(WindDirection windDirection, int winStrength)
    {
        ClearIndicator();
        GameObject prefabWind;
        if (windDirection == WindDirection.LEFT)
        {
            prefabWind = leftWindDirection;
        } else
        {
            prefabWind = rightWindDirection;
        }

        for (int icon = 1; icon <= winStrength; icon++)
        {
            Renderer renderer = prefabWind.GetComponent<Renderer>();
            Vector3 positonWindIcon = new Vector3(transform.position.x + renderer.bounds.extents.x * 2 * icon,
                transform.position.y);
            GameObject iconObject = Instantiate(prefabWind, positonWindIcon, Quaternion.identity);
            iconObject.transform.SetParent(transform);
        }
    }

    private void ClearIndicator()
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}
