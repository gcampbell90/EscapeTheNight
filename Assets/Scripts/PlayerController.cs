using AmazingAssets.CurvedWorld.Example;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(VelocityTracker))]
[RequireComponent(typeof(BoostController))]

public class PlayerController : MonoBehaviour
{
    //UI
    [SerializeField] private TextMeshProUGUI _gpsInfo;
    [SerializeField] private Slider _progressSlider;
    [SerializeField] private Slider _boostSlider;

    //Scene spawner speed
    public ChunkSpawner[] chunkSpawners;
    public ChunkSpawner[] ChunkSpawners { get { return chunkSpawners; } set { ChunkSpawners = chunkSpawners; } }

    public float StandardSpeed { get; set; }
    public float BoostSpeed { get; set; }
    public float CurrentSpeed { get; set; }

    private BoostController boostController;
    public BoostController BoostController { get { return boostController; } set { boostController = BoostController; } }

    private VelocityTracker velocityTracker;

     private void Awake()
    {
        boostController = GetComponent<BoostController>();
        velocityTracker = GetComponent<VelocityTracker>();
    }

    private void Start()
    {
        StandardSpeed = VelocityTracker.GetMeterPerSec(GameController.Instance.StandardSpeed_MPH);
        BoostSpeed = VelocityTracker.GetMeterPerSec(GameController.Instance.BoostSpeed_MPH);

        CurrentSpeed = StandardSpeed;
        //Debug.Log($"Normal m/s: {VelocityTracker.GetMeterPerSec(GameController.Instance.StandardSpeed_MPH)} Boost m:s: {VelocityTracker.GetMeterPerSec(GameController.Instance.BoostSpeed_MPH)} CurrentSpeed: {CurrentSpeed}");
    }

    public void UpdateUI(float countDown, float eta, string message, Color col, float distanceRemaining, float progress)
    {
        _gpsInfo.color = col;
        _gpsInfo.text = $"{message} Gate Closing in {countDown.ToString("#.##")}\nDistance Rem(Miles):{distanceRemaining.ToString(".##")}\nETA: {eta.ToString("#.##")}";
        _progressSlider.value = progress;
    }

    public void UpdateUI(float progress)
    {
        progress /= 100;
        _boostSlider.value = progress;
    }

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log($"Player Hit something");

    }

    ////Sets game controllers new speed
    //public void SpeedChange(float newSpeed)
    //{
    //    Debug.Log("Changing Speed player controller " + newSpeed);
    //    CurrentSpeed = newSpeed;
    //    GameController.Instance.EnvironmentSpeedChange(newSpeed);
    //}
}
