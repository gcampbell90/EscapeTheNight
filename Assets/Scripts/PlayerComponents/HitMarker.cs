using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitMarker : MonoBehaviour
{
    public static int markerCount;

    private void OnEnable()
    {
        markerCount++;
        StartCoroutine(DestroyOnSec(0.5f));
        //Debug.Log(markerCount);
    }

    private IEnumerator DestroyOnSec(float v)
    {
        var timer = 0f;
        while(timer <= v)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
        markerCount--;
        //Debug.Log(markerCount);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

}
