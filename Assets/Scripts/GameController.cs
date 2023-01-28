using AmazingAssets.CurvedWorld.Example;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Range(70, 100)]
    [SerializeField] private float goalTimePercentage;
    public float GoalTimePercentage { get => goalTimePercentage; private set => goalTimePercentage = value; }

    [Header("Player Setup")]
    [SerializeField] private float standardSpeed_MPH;
    public float StandardSpeed_MPH { get => standardSpeed_MPH; private set => standardSpeed_MPH = value; }

    [SerializeField] private float boostSpeed_MPH;
    public float BoostSpeed_MPH { get => boostSpeed_MPH; private set => boostSpeed_MPH = value; }

    [SerializeField] private float penaltySpeed;
    public float PenaltySpeed { get => penaltySpeed; private set => penaltySpeed = value; }

    [Header("Level Setup")]
    [SerializeField] private float levelLength_Miles;

    //environment chunks
    [SerializeField] private ChunkSpawner[] chunkSpawners;

    public float LevelLength_Miles { get => levelLength_Miles; private set => levelLength_Miles = value; }

    [SerializeField] private GameObject endWallPrefab;

    //Event for when the player boosts - speeds up scene and appropriate other objects
    public delegate void OnSpeedChange(float newSpeed);
    public static OnSpeedChange onSpeedChange;

    public static GameController Instance { get; private set; }

    public TelemetryCalculatorBehaviour TelemetryTracker { get; set; } 

    private void OnEnable()
    {
        onSpeedChange += SpeedUpdateEvent;
    }
    private void OnDisable()
    {
        onSpeedChange -= SpeedUpdateEvent;
    }

    private void Awake()
    {
        TelemetryTracker = GetComponent<TelemetryCalculatorBehaviour>();
        //uiController = GetComponent<UIController>();

        Instance = this;
        //Debug.Log($"StartMPH:{standardSpeed_MPH},BoostSpeed: {boostSpeed_MPH}, Length of road: {levelLength_Miles}");
    }
    private void Start()
    {
        var gateSpawnPos = new Vector3(0, 0, LevelLength_Miles * 1609.34f);

        Instantiate(endWallPrefab, gateSpawnPos, Quaternion.identity);

        foreach (var chunk in chunkSpawners)
        {
            chunk.movingSpeed = StandardSpeed_MPH * 0.44704f;
        }
    }

    private void SpeedUpdateEvent(float newSpeed)
    {
        StartCoroutine(SpeedUpdate(newSpeed));
    }
    private IEnumerator SpeedUpdate(float newSpeed)
    {
        var _t = 0f;
        var _dur = 1f;
        while (_t < 1f)
        {
            TelemetryTracker.speed = newSpeed;

            foreach (var chunk in chunkSpawners)
            {
                chunk.movingSpeed = newSpeed * 0.44704f;
                _t += Time.deltaTime / _dur;
            }
            yield return null;
        }
    }

}

