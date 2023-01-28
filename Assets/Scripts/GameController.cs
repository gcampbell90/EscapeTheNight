using AmazingAssets.CurvedWorld.Example;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameController : MonoBehaviour
{
    //Singleton
    private static GameController _instance;
    public static GameController Instance { get { return _instance; } }

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

    //Event for when the player boosts - speeds up scene and appropriate other objects
    public delegate void OnGameOver();
    public static OnGameOver onGameOver;

    public TelemetryCalculatorBehaviour TelemetryTracker { get; set; }

    private void OnEnable()
    {
        onSpeedChange += SpeedUpdateEvent;
        onGameOver += GameOverEvent;
    }

    private void OnDisable()
    {
        onSpeedChange -= SpeedUpdateEvent;
        onGameOver -= GameOverEvent;
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        TelemetryTracker = GetComponent<TelemetryCalculatorBehaviour>();
        //Debug.Log($"StartMPH:{standardSpeed_MPH},BoostSpeed: {boostSpeed_MPH}, Length of road: {levelLength_Miles}");
    }
    private async void Start()
    {
        var gateSpawnPos = new Vector3(0, 0, LevelLength_Miles * 1609.34f);

        var endWall = Instantiate(endWallPrefab, gateSpawnPos, Quaternion.identity);
        
        //When loading main scene and unloading intro scene we have to move the wall GO.
        //When just playing the main scene this will throw and error
        SceneController.Instance.MoveGameObject(endWall);

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
    private async void GameOverEvent()
    {
        Debug.Log("GameOver event");
        await SceneController.Instance.LoadSceneAsync("OutroScene");
        await SceneController.Instance.UnloadSceneAsync("MainScene");
    }
}

