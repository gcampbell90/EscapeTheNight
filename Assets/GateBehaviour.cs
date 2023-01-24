using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateBehaviour : MonoBehaviour
{
    private TelemetryCalculatorBehaviour telemetryCalculator;

    private void Awake()
    {
        telemetryCalculator = GameController.Instance.TelemetryTracker;
    }

    private void Start()
    {
        StartCoroutine(MoveWall());
    }

    private IEnumerator MoveWall()
    {
        var gcInstance = GameController.Instance;
        Vector3 gatePos = new Vector3(0, 0, gcInstance.LevelLength_Miles * 1609.34f);

        Debug.Log($"gatepos: {gatePos} remainingDist:{telemetryCalculator.RemainingDistance}");
        while (true)
        {
            gatePos.z = telemetryCalculator.RemainingDistance * 1609.34f;
            transform.position = gatePos;
            //Debug.Log("Moving wall" + (etaTracker.RemainingDistance * 1609.34f));
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log($"Gate Enter" + collision.name);
    }
}
