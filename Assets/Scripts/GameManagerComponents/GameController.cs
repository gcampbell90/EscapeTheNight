using AmazingAssets.CurvedWorld.Example;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private static GameController _instance;
    public static GameController Instance { get { return _instance; } }

    [Range(30, 100)]
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
    [SerializeField] private ChunkSpawner[] environmentSpawners;
    [SerializeField] private CarChunkSpawner[] carSpawners;

    public float LevelLength_Miles { get => levelLength_Miles; private set => levelLength_Miles = value; }

    [SerializeField] private GameObject endWallPrefab;

    GameObject gateGO;
    GateBehaviour gateScript;

    #region Events
    //Event for when the player boosts - speeds up scene and appropriate other objects
    public delegate void OnSpeedChange(float newSpeed);
    public static OnSpeedChange onSpeedChange;

    public delegate void OnGameOver();
    public static OnGameOver onGameOver;

    public delegate void OnGameComplete();
    public static OnGameComplete onGameComplete;
    #endregion

    public TelemetryCalculatorBehaviour TelemetryTracker { get; set; }

    public bool isWinner { get; set; }

    private void OnEnable()
    {
        onSpeedChange += SpeedUpdateEvent;
        onGameOver += GameOverEvent;
        onGameComplete += GameCompleteEvent;
    }
    private void OnDisable()
    {
        onSpeedChange -= SpeedUpdateEvent;
        onGameOver -= GameOverEvent;
        onGameComplete -= GameCompleteEvent;

    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }

        TelemetryTracker = GetComponent<TelemetryCalculatorBehaviour>();
        DontDestroyOnLoad(gameObject);
        //Debug.Log($"StartMPH:{standardSpeed_MPH},BoostSpeed: {boostSpeed_MPH}, Length of road: {levelLength_Miles}");
    }
    private void Start()
    {
        Time.timeScale = 1f;
        SpawnEndWall();
        SpeedUpdateEvent(StandardSpeed_MPH);
    }


    private void SpawnEndWall()
    {
        var gateSpawnPos = new Vector3(0, 0, LevelLength_Miles * 1609.34f);
        gateGO = Instantiate(endWallPrefab, gateSpawnPos, Quaternion.identity);

        if (SceneController.Instance == null) return;
        SceneController.Instance.MoveGameObject(gateGO);
    }
    private void SpeedUpdateEvent(float newSpeed)
    {
        TelemetryTracker.Speed = newSpeed;

        if (gateScript == null)
        {
            gateScript = gateGO.GetComponent<GateBehaviour>();
        }
        gateScript.MoveSpeed = (newSpeed * 0.44704f);

        foreach (var chunk in environmentSpawners)
        {
            chunk.movingSpeed = (newSpeed * 0.44704f);
        }
        //28.2f added to account for cars already moving at 60mph anyway
        foreach (var chunk in carSpawners)
        {
            chunk.movingSpeed = (newSpeed * 0.44704f) + 28.2f;
        }

        UIController.onUiStateChange(GetUIState(newSpeed));

    }

    private UIController.UIState GetUIState(float speed)
    {
        if (speed == standardSpeed_MPH)
        {
            return UIController.UIState.Normal;
        }
        else if (speed == boostSpeed_MPH)
        {
            return UIController.UIState.Boost;
        }
        else
        {
            return UIController.UIState.Penalty;
        }
    }

    //private IEnumerator SpeedUpdate(float newSpeed)
    //{
    //    var _t = 0f;
    //    var _dur = 1f;
    //    GateBehaviour gateScript = gateGO.GetComponent<GateBehaviour>();
    //    while (_t < 1f)
    //    {
    //        TelemetryTracker.Speed = newSpeed;

    //        gateScript.MoveSpeed = (newSpeed * 0.44704f);

    //        //foreach (var chunk in carSpawners)
    //        //{
    //        //    chunk.movingSpeed = (newSpeed * 0.44704f) + 28.82f;
    //        //}

    //        foreach (var chunk in environmentSpawners)
    //        {
    //            chunk.movingSpeed = (newSpeed * 0.44704f);
    //        }

    //        _t += Time.deltaTime / _dur;

    //        yield return null;
    //    }
    //}
    private void GameOverEvent()
    {
        //Debug.Log("GameOver event");
        isWinner = false;
        EndGame();
    }
    private void GameCompleteEvent()
    {
        isWinner = true;
        EndGame();
    }
    private async void EndGame()
    {
        if (SceneController.Instance == null) return;
        await SceneController.Instance.LoadOutroScene();
        Destroy(gameObject);
    }
}

