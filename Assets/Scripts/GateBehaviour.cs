using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateBehaviour : MonoBehaviour
{
    private TelemetryCalculatorBehaviour telemetryTracker;

    private void Awake()
    {
        telemetryTracker = GameController.Instance.TelemetryTracker;
    }

    private void Start()
    {
        StartCoroutine(MoveWall());
    }

    private IEnumerator MoveWall()
    {
        Vector3 gatePos = new Vector3(0, 0, GameController.Instance.LevelLength_Miles * 1609.34f);

        while (true)
        {
            gatePos.z = telemetryTracker.RemainingDistance * 1609.34f;
            transform.position = gatePos;
            //Debug.Log($"gatepos: {gatePos} remainingDist:{telemetryCalculator.RemainingDistance}");
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log($"Gate Enter " + collision.name);
        GameController.onGameOver();
    }
}
