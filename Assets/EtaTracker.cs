using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EtaTracker : MonoBehaviour
{
    // speed in mph
    public float speed;
    // distance in miles
    public float distance;
    // distance traveled so far
    private float distanceTraveled = 0f;

    public TextMeshProUGUI etaText;

    void Update()
    {
        // update the distance traveled based on the speed and elapsed time
        distanceTraveled += speed * Time.deltaTime / 3600f;
        // calculate the remaining distance
        float remainingDistance = distance - distanceTraveled;
        // calculate the ETA based on the remaining distance and speed
        float eta = remainingDistance / speed;
        // convert the ETA from hours to minutes
        int etaMinutes = (int)(eta * 60);
        // calculate the number of seconds
        int etaSeconds = (int)((eta * 60 - etaMinutes) * 60);
        etaText.text = etaMinutes + " minutes" + " " + etaSeconds + " seconds";
        // print the ETA value to the console
        //Debug.Log(etaMinutes + " minutes" + " " + etaSeconds + " seconds");
    }
}
