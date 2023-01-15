using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Movement
    bool dir;
    float rot;
    float pos;
    Vector3 targetPosition;

    private Vector3 middlePosition = new Vector3(0, 0, 0);
    private Vector3 leftPosition = new Vector3(-5, 0, 0);
    private Vector3 rightPosition = new Vector3(5, 0, 0);

    private enum CurrentPos { middle, left, right };
    private CurrentPos currLanePosition;
    public Vector3 TartgetPos => SetTargetPosition();
    public Quaternion TargetRot => SetTargetRotation();

    [SerializeField]private float laneChangeSpeed; 

    private void Start()
    {
        currLanePosition = CurrentPos.middle;
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
            if (currLanePosition == CurrentPos.middle)
            {
                targetPosition = leftPosition;
                currLanePosition = CurrentPos.left;
            }
            else if (currLanePosition == CurrentPos.right)
            {
                targetPosition = middlePosition;
                currLanePosition = CurrentPos.middle;
            }
            else
            {
                return;
            }
        }

        //right
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (currLanePosition == CurrentPos.middle)
            {
                targetPosition = rightPosition;
                currLanePosition = CurrentPos.right;

            }
            else if (currLanePosition == CurrentPos.left)
            {
                targetPosition = middlePosition;
                currLanePosition = CurrentPos.middle;
            }
            else
            {
                return;
            }
        }

        //boost
        if (Input.GetKeyDown(KeyCode.W))
        {
            BoostController.boostEvent(true);
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            BoostController.boostEvent(false);
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, laneChangeSpeed * Time.deltaTime);

    }
    Vector3 SetTargetPosition()
    {
        pos = dir ? -4 : 4 ;
        return new Vector3(pos, 0, 0);
    }
    Quaternion SetTargetRotation()
    {
        rot = dir ? -25 : 25;
        return Quaternion.Euler(0, rot, 0);
    }

    private void PlayerTurnSFX()
    {
        var audioController = GetComponent<AudioSourceController>();
        audioController.PlayTireScreech();
    }
}
