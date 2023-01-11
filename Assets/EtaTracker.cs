using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using static GameController;

public class EtaTracker : MonoBehaviour
{
    // speed in mph
    public float speed { get; set; }
    // distance in miles
    public float distance { get; set; }

    private float distanceTraveled = 0f;
    private float goalTime;
    public float RemainingDistance { get; set; }
    float eta;

    UIController uiController { get; set; }

    private void Start()
    {
        uiController = GetComponent<UIController>();
        goalTime = (speed / distance) * 0.8f;
    }

    void Update()
    {
        distanceTraveled += speed * Time.deltaTime / 3600f;
        RemainingDistance = distance - distanceTraveled;
        eta = RemainingDistance / speed;
        int etaSeconds = (int)((eta * 60) * 60);

        goalTime -= Time.deltaTime;

        uiController.UpdateUI(etaSeconds, distanceTraveled);
        
        if (etaSeconds <= 0)
        {
            Debug.Log("Times Up");
        }
    }
}
