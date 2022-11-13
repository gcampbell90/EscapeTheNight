using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitMarker : MonoBehaviour
{
    public static int markerCount;

    private void OnEnable()
    {
        markerCount++;
        Debug.Log(markerCount);
    }

    private void Update()
    {
        if (!gameObject.activeInHierarchy)
        {
            Destroy(gameObject);
            markerCount--;
        }
    }

}
