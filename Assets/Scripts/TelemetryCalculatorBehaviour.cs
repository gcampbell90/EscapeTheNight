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
    public float RemainingDistance { get; set; }

    float eta;
    private float goalTime;

    int etaSeconds;
    float goalTimeSeconds;
    UIController uiController { get; set; }

    private void Start()
    {
        uiController = GetComponent<UIController>();

        distance = Instance.LevelLength_Miles;
        speed = Instance.StandardSpeed_MPH;

        RemainingDistance = distance - distanceTraveled;

        eta = RemainingDistance / speed;
        goalTime = eta * (Instance.GoalTimePercentage / 100);
    }

    void Update()
    {
        distanceTraveled += speed * Time.deltaTime / 3600f;
        RemainingDistance = distance - distanceTraveled;

        goalTime -= Time.deltaTime;

        etaSeconds = (int)((eta * 60) * 60);
        goalTimeSeconds = (int)((goalTime * 60) * 60);
        var progress = distance / distanceTraveled;
        uiController.UpdateUI(speed, etaSeconds, goalTimeSeconds, progress);

        if (etaSeconds <= 0)
        {
            Debug.Log("Times Up");
        }
    }
}
