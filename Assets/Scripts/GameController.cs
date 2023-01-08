using AmazingAssets.CurvedWorld.Example;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static GameController;

public class GameController : MonoBehaviour
{
    [Header("Player Setup")]
    [SerializeField] PlayerController playerController;
    [SerializeField] private float standardSpeed_MPH;
    [SerializeField] private float boostSpeed_MPH;
    public float StandardSpeed_MPH { get => standardSpeed_MPH; private set => standardSpeed_MPH = value; }
    public float BoostSpeed_MPH { get => boostSpeed_MPH; private set => boostSpeed_MPH = value; }

    [Header("Level Setup")]
    [SerializeField] private float levelLength_Miles;
    //Scene spawner speed
    [SerializeField] private ChunkSpawner[] chunkSpawners;
    //[SerializeField] private ChunkSpawner[] ChunkSpawners { get { return chunkSpawners; } set { ChunkSpawners = chunkSpawners; } }

    public float LevelLength_Miles { get => levelLength_Miles; private set => levelLength_Miles = value; }

    //Event for when the player boosts - speeds up scene and appropriate other objects
    public delegate void OnSpeedChange(float newSpeed);
    public static OnSpeedChange onSpeedChange;

    public static GameController Instance { get; private set; }


    private void OnEnable()
    {
        onSpeedChange += EnvironmentSpeedChange;
    }

    private void OnDisable()
    {
        onSpeedChange -= EnvironmentSpeedChange;
    }

    private void Awake()
    {
        Instance = this;
        //Debug.Log($"StartMPH:{standardSpeed_MPH},BoostSpeed: {boostSpeed_MPH}, Length of road: {levelLength_Miles}");
    }

    private void Start()
    {
        foreach (var chunk in chunkSpawners)
        {
            chunk.movingSpeed = VelocityTracker.GetMeterPerSec(StandardSpeed_MPH);
        }
    }

    public void EnvironmentSpeedChange(float newSpeed)
    {
        StartCoroutine(SpeedUpdate(newSpeed));
        playerController.CurrentSpeed = newSpeed;
    }

    private IEnumerator SpeedUpdate(float newSpeed)
    {
        var _t = 0f;
        var _dur = 1f;
        while (_t < 1f)
        {
            foreach (var chunk in chunkSpawners)
            {
                chunk.movingSpeed = newSpeed;
                _t += Time.deltaTime / _dur;
            }
            yield return null;
        }
    }
}

