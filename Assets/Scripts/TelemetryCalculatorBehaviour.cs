using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using static GameController;

public class TelemetryCalculatorBehaviour : MonoBehaviour
{
    // speed in mph
    public float speed { get; set; }
    // distance in miles
    public float distance { get; set; }

    private float distanceTraveled = 0f;
    public float RemainingDistance
    {
        get
        {
            return distance - distanceTraveled;
        }
        set
        {
            distance = value;
        }
    }

    private float eta;
    private float goalTime;

    int etaSeconds;
    float goalTimeSeconds;

    private void Start()
    {
        distance = Instance.LevelLength_Miles;
        speed = Instance.StandardSpeed_MPH;

        eta = distance / 100;
        goalTime = eta;
    }

    void Update()
    {
        distanceTraveled += speed * Time.deltaTime / 3600f;
        eta = RemainingDistance / speed;

        var percentage = Instance.GoalTimePercentage / 100;
        //goalTime = eta * percentage;
        goalTime -= Time.deltaTime;

        etaSeconds = (int)((eta * 60) * 60);
        goalTimeSeconds = (int)(((goalTime * (Instance.GoalTimePercentage / 100)) * 60) * 60);

        var progress = distance / distanceTraveled;

        UIController.onUIChange.Invoke(speed, etaSeconds, goalTimeSeconds, progress);

        //Debug.Log("Eta: " + etaSeconds + " " + "GoalTime: " + " " + goalTimeSeconds + " " + Instance.GoalTimePercentage + "%");
        //if (goalTime <= 0)
        //{
        //    Debug.Log("Times Up");
        //}
    }
}
