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
    float borderRange = 8;

    [SerializeField] private float normal_MPH;
    public float Normal_MPH { get { return normal_MPH; } set { normal_MPH = Normal_MPH; } }

    [SerializeField] private float boost_MPH;
    public float Boost_MPH { get; set; }
    public float NormalSpeed { get; set; }
    public float BoostSpeed { get; set; }
    public float Speed { get; set; }

    private BoostController boostController;
    public BoostController BoostController { get { return boostController; } set { boostController = BoostController; } }

    private VelocityTracker velocityTracker;

    private void Awake()
    {

        boostController = GetComponent<BoostController>();
        velocityTracker = GetComponent<VelocityTracker>();

        //Changes MPH input from player controller to m/s which will be the unit of measure
        NormalSpeed = BoostSpeed = velocityTracker.GetMeterPerSec(normal_MPH);
        BoostSpeed = velocityTracker.GetMeterPerSec(boost_MPH);

        Speed = NormalSpeed;

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

}
