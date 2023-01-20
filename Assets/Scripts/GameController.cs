using AmazingAssets.CurvedWorld.Example;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Range(0,100)]
    [SerializeField] private float goalTimePercentage;
    [Header("Player Setup")]
    [SerializeField]private float standardSpeed_MPH;
    public float StandardSpeed_MPH { get => standardSpeed_MPH; private set => standardSpeed_MPH = value; }

    [SerializeField] private float boostSpeed_MPH;
    public float BoostSpeed_MPH { get => boostSpeed_MPH; private set => boostSpeed_MPH = value; }

    [Header("Level Setup")]
    [SerializeField] private float levelLength_Miles;

    //environment chunks
    [SerializeField] private ChunkSpawner[] chunkSpawners;

    public float LevelLength_Miles { get => levelLength_Miles; private set => levelLength_Miles = value; }
    
    private GameObject endWall;

    //Event for when the player boosts - speeds up scene and appropriate other objects
    public delegate void OnSpeedChange(float newSpeed);
    public static OnSpeedChange onSpeedChange;

    public static GameController Instance { get; private set; }

    private TelemetryCalculatorBehaviour etaTracker;
    //implement
    private UIController uiController;

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
        etaTracker = GetComponent<TelemetryCalculatorBehaviour>();
        uiController = GetComponent<UIController>();

        Instance = this;
        //Debug.Log($"StartMPH:{standardSpeed_MPH},BoostSpeed: {boostSpeed_MPH}, Length of road: {levelLength_Miles}");
    }
    private void Start()
    {
        endWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        endWall.transform.localScale = new Vector3(20, 20, 1);
        endWall.transform.position = new Vector3(0, 0, levelLength_Miles * 1609.34f);
        StartCoroutine(MoveWall());
        foreach (var chunk in chunkSpawners)
        {
            chunk.movingSpeed = StandardSpeed_MPH * 0.44704f;
        }
        etaTracker.distance = levelLength_Miles;
        etaTracker.speed = standardSpeed_MPH;
    }

    private IEnumerator MoveWall()
    {
        while (true)
        {
            endWall.transform.position = transform.position + new Vector3(0, 0, etaTracker.RemainingDistance * 1609.34f);
            //Debug.Log("Moving wall" + (etaTracker.RemainingDistance * 1609.34f));
            yield return null;
        }
    }
    public void SpeedUpdateEvent(float newSpeed)
    {
        StartCoroutine(SpeedUpdate(newSpeed));
    }
    private IEnumerator SpeedUpdate(float newSpeed)
    {
        var _t = 0f;
        var _dur = 1f;
        while (_t < 1f)
        {
            etaTracker.speed = newSpeed;

            foreach (var chunk in chunkSpawners)
            {
                chunk.movingSpeed = newSpeed;
                _t += Time.deltaTime / _dur;
            }
            yield return null;
        }
    }

}

