using System.Collections;
using UnityEngine;


public class VelocityTracker : MonoBehaviour
{
    private PlayerController playerController;

    private float totalLevelDistanceMiles;
    public float DistanceCovered { get; set; }
    public float TotalDistanceRemaining { get; set; }

    private const float METER_TO_MILE = 0.00062f;
    private const float MPH_TO_MS = 0.44704f;
    //private const float msToMph = 2.2369f;

    private float _eta;
    private float _targetEta;

    float distanceNormal, distanceBoost;
    float timer;

    private void Awake()
    {
        totalLevelDistanceMiles = GameController.Instance.LevelLength_Miles;
    } 
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        StartCoroutine(CalculateMetrics());
    }

    private IEnumerator CalculateMetrics()
    {
        float countDown;
        string message;
        Color col;
        float _normalTimer = 0f;
        float _boostTimer = 0f;

        var boostController = playerController.BoostController;

        while (true)
        {
            if (!boostController.IsBoosting)
            {
                //Debug.Log("normal vel:");
                playerController.Speed = playerController.CurrentSpeed;
                distanceNormal = playerController.Speed * _normalTimer;
                _normalTimer += Time.deltaTime;
            }
            else if (boostController.IsBoosting)
            {
                //Debug.Log("Boost vel:");
                playerController.Speed = playerController.BoostSpeed;
                distanceBoost = playerController.Speed * _boostTimer;
                _boostTimer += Time.deltaTime;
            }

            DistanceCovered = (distanceNormal * METER_TO_MILE) + (distanceBoost * METER_TO_MILE);

            TotalDistanceRemaining = totalLevelDistanceMiles - DistanceCovered;

            _eta = (TotalDistanceRemaining / GameController.Instance.StandardSpeed_MPH);

            _targetEta = ((totalLevelDistanceMiles / GameController.Instance.StandardSpeed_MPH) * 0.8f);
            countDown = ToSeconds(_targetEta) - timer;
            var etaSecs = ToSeconds(_eta);

            message = (etaSecs < countDown) ?
            "Good - Dont let up!" :
                "Hurry!";

            col = (etaSecs < countDown) ?
                Color.green :
            Color.red;

            timer += Time.deltaTime;


            var progress = DistanceCovered / totalLevelDistanceMiles;

            playerController.UpdateUI(countDown, etaSecs, message, col, TotalDistanceRemaining, progress);
            //Debug.Log($"{message}, {_eta}, {_targetEta}, {countDown}, {progress}");

            yield return null;
        }
    }
    float ToSeconds(float eta)
    {
        eta *= 60;
        eta *= 60;
        var seconds = eta;
        return seconds;
    }
    public float GetMeterPerSec(float mph)
    {
        return (mph * MPH_TO_MS);
    }
}