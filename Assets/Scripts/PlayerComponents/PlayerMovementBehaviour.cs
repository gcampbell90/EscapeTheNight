using System;
using System.Collections;
using UnityEngine;

public class PlayerMovementBehaviour : MonoBehaviour
{
    Vector3 targetPosition;

    private Vector3 middlePosition = new Vector3(0, 0.5f, 0);
    private Vector3 leftPosition = new Vector3(-5, 0.5f, 0);
    private Vector3 rightPosition = new Vector3(5, 0.5f, 0);

    private enum CurrentPos { middle, left, right };
    private CurrentPos currLanePosition;

    [SerializeField] private float laneChangeSpeed;

    public delegate void OnPlayerHit();
    public static OnPlayerHit onPlayerHit;


    private void OnEnable()
    {
        onPlayerHit += HitPenalty;
    }

    private void OnDisable()
    {
        onPlayerHit -= HitPenalty;    
    }

    private void Start()
    {
        currLanePosition = CurrentPos.middle;
        targetPosition = middlePosition;
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {

        //left
        if (Input.GetKeyDown(KeyCode.A))
        {
            switch (currLanePosition)
            {
                case CurrentPos.middle:
                    targetPosition = leftPosition;
                    currLanePosition = CurrentPos.left;
                    break;
                case CurrentPos.right:
                    targetPosition = middlePosition;
                    currLanePosition = CurrentPos.middle;
                    break;
                default:
                    return;
            }
        }

        //right
        if (Input.GetKeyDown(KeyCode.D))
        {
            switch (currLanePosition)
            {
                case CurrentPos.middle:
                    targetPosition = rightPosition;
                    currLanePosition = CurrentPos.right;
                    break;
                case CurrentPos.left:
                    targetPosition = middlePosition;
                    currLanePosition = CurrentPos.middle;
                    break;
                default:
                    return;
            }
        }

        //boost
        if (Input.GetKeyDown(KeyCode.W))
        {
            BoostBehaviour.boostEvent?.Invoke(true);
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            BoostBehaviour.boostEvent?.Invoke(false);
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, laneChangeSpeed * Time.deltaTime);

    }

    public void HitPenalty()
    {
        targetPosition = middlePosition;
        currLanePosition = CurrentPos.middle;
        GameController.onSpeedChange?.Invoke(GameController.Instance.PenaltySpeed);
    }
}
