using UnityEngine;
using static GameController;

public class TelemetryCalculatorBehaviour : MonoBehaviour
{
    // speed in mph
    public float Speed { get; set; }
    // distance in miles
    public float Distance { get; set; }

    private float distanceTraveled = 0f;
    public float RemainingDistance
    {
        get
        {
            return Distance - distanceTraveled;
        }
        set
        {
            Distance = value;
        }
    }

    private float eta;
    private float goalTime;

    public int etaSeconds;
    public int goalTimeSeconds;
    float timer;

    bool gameOver = false;
    private void Start()
    {
        Distance = Instance.LevelLength_Miles;
        Speed = Instance.StandardSpeed_MPH;

        //Times
        timer = 0f;
        eta = Distance / Speed;
        goalTime = eta;

        etaSeconds = (int)((eta * 60) * 60);
        goalTimeSeconds = (int)(((goalTime * (Instance.GoalTimePercentage / 100)) * 60) * 60);

        //Debug.Log($"Start - Dist {Distance} - Speed {Speed}");
        //Debug.Log($"Start - Eta {eta} - Goaltime {goalTime}");
        //Debug.Log($"Start - ToSeconds Eta {etaSeconds} - GoalTime{goalTimeSeconds}");
    }

    void Update()
    {
        distanceTraveled += Speed * Time.deltaTime / 3600f;
        eta = RemainingDistance / Speed;

        var percentage = Instance.GoalTimePercentage / 100;
        //goalTime = eta * percentage;

        etaSeconds = (int)((eta * 60) * 60);

        goalTimeSeconds = (int)(((goalTime * (Instance.GoalTimePercentage / 100)) * 60) * 60);
        goalTimeSeconds -= (int)timer;
        var progress = distanceTraveled / Distance;

        progress = Mathf.Clamp01(progress);

        UIController.onUIChange?.Invoke(Speed, etaSeconds, goalTimeSeconds, progress);

        if (goalTimeSeconds <= 0)
        {
            if (gameOver) return;

            //Debug.Log("Times Up");
            gameOver = true;
            onGameOver?.Invoke();
        }

        timer += Time.deltaTime;
        //Debug.Log("Eta: " + etaSeconds + " " + "GoalTime: " + " " + goalTimeSeconds + " " + Instance.GoalTimePercentage + "%");
    }
}
