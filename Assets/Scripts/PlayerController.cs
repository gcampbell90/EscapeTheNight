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

    //Scene spawner speed
    public ChunkSpawner[] chunkSpawners;
    public ChunkSpawner[] ChunkSpawners { get { return chunkSpawners; } set { ChunkSpawners = chunkSpawners; } }

    public float CurrentSpeed => velocityTracker.GetMeterPerSec(GameController.Instance.StandardSpeed_MPH);
    public float BoostSpeed => velocityTracker.GetMeterPerSec(GameController.Instance.BoostSpeed_MPH);
    public float Speed { get; set; }

    private BoostController boostController;
    public BoostController BoostController { get { return boostController; } set { boostController = BoostController; } }

    private VelocityTracker velocityTracker;

    public delegate void DoSomething();
    public static event DoSomething doSomething;

    private void Awake()
    {
        boostController = GetComponent<BoostController>();
        velocityTracker = GetComponent<VelocityTracker>();

        Speed = CurrentSpeed;

    }

    private void Start()
    {
        foreach (var chunk in chunkSpawners)
        {
            chunk.movingSpeed = Speed;
        }
    }

    public void UpdateUI(float countDown, float eta, string message, Color col, float distanceRemaining, float progress)
    {
        _gpsInfo.color = col;
        _gpsInfo.text = $"{message} Gate Closing in {countDown.ToString("#.##")}\nDistance Rem(Miles):{distanceRemaining.ToString(".##")}\nETA: {eta.ToString("#.##")}";
        _progressSlider.value = progress;
    }

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log($"Vehicle Hit by {collision}");

    }
}
