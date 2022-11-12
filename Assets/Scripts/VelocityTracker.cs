using System.Collections;
using UnityEngine;


public class VelocityTracker : MonoBehaviour
{
    PlayerController playerController;

    public float totalLevelDistanceMiles;
    public float DistanceCovered { get; set; }
    public float TotalDistanceRemaining { get; set; }

    private const float _meterToMile = 0.00062f;
    private const float _mphToMs = 0.44704f;
    //private const float msToMph = 2.2369f;

    private float _eta;
    private float _targetEta;

    float distanceNormal, distanceBoost;
    float timer;

    public float GetMeterPerSec(float mph)
    {
        return (mph * _mphToMs);
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
        var mph = playerController.NormalSpeed;

        while (true)
        {
            if (!boostController.IsBoosting)
            {
                //Debug.Log("normal vel:");
                playerController.Speed = playerController.NormalSpeed;
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

            DistanceCovered = (distanceNormal * _meterToMile) + (distanceBoost * _meterToMile);

            TotalDistanceRemaining = totalLevelDistanceMiles - DistanceCovered;

            _eta = (TotalDistanceRemaining / playerController.Normal_MPH);

            _targetEta = ((totalLevelDistanceMiles / playerController.Normal_MPH) * 0.8f);
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

            //Debug.Log($"{message}, {_eta}, {_targetEta}, {countDown}, {progress}");
            playerController.UpdateUI(countDown, etaSecs, message, col, TotalDistanceRemaining, progress);
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

}