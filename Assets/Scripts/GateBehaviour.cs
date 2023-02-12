using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateBehaviour : MonoBehaviour
{
    private TelemetryCalculatorBehaviour telemetryTracker;
    public float MoveSpeed { get; set; }
    private void Awake()
    {
        telemetryTracker = GameController.Instance.TelemetryTracker;
    }

    private void Start()
    {
        //StartCoroutine(MoveWall());
    }

    private void Update()
    {
        var moveDirection = new Vector3(0, 0, -1);

        transform.Translate(moveDirection * MoveSpeed * Time.deltaTime);

    }

    private IEnumerator MoveWall()
    {
        Vector3 gatePos = new Vector3(0, 0, (GameController.Instance.LevelLength_Miles * 1609.34f)*2);
        var moveDirection = new Vector3(0, 0, -1);
        while (true)
        {
            transform.Translate(moveDirection * MoveSpeed * Time.deltaTime);
            
            //Debug.Log($"gatepos: {gatePos} remainingDist:{telemetryCalculator.RemainingDistance}");
            yield return null;
        }
    }

    void UpdatePos()
    {

    }

    private void OnTriggerEnter(Collider collision)
    {
        //Debug.Log($"Gate Enter " + collision.name);
        GameController.onGameComplete?.Invoke();
    }
}
