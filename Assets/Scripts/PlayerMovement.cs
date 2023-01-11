using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerController playerController;

    //Coroutine moveRoutine = null;

    //Movement
    bool dir;
    float rot;
    float pos;

    private Vector3 middlePosition = new Vector3(0, 0, 0);
    private Vector3 leftPosition = new Vector3(-5, 0, 0);
    private Vector3 rightPosition = new Vector3(5, 0, 0);

    Vector3 targetPosition;
    public Vector3 TartgetPos => SetTargetPosition();
    public Quaternion TargetRot => SetTargetRotation();

    private enum CurrentPos { middle, left, right };
    private CurrentPos currLanePosition;

    [SerializeField] private float speed;
    //bool isRunning = false;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        currLanePosition = CurrentPos.middle;
    }
    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {

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
        else if (Input.GetKeyDown(KeyCode.D))
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
        else if (Input.GetKeyDown(KeyCode.W))
        {
            playerController.BoostController.Booster();
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        //if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        //{
        //    //Debug.Log("Key Pressed");
        //    if (Input.GetKeyDown(KeyCode.W))
        //    {

        //        if (isRunning) { StopCoroutine(moveRoutine); };
        //        playerController.BoostController.Booster();
        //    }

        //    if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        //    {
        //        //Dont allow turning if boosting
        //        if (playerController.BoostController.IsBoosting) return;

        //        if (Input.GetKeyDown(KeyCode.A))
        //        {
        //            //Debug.Log("Turning Left");

        //            dir = true;

        //        }
        //        else if (Input.GetKeyDown(KeyCode.D))
        //        {
        //            //Debug.Log("Turning Right");

        //            dir = false;
        //        }
        //        else
        //        {
        //            return;
        //        }

        //        if (isRunning && moveRoutine != null)
        //        {
        //            StopCoroutine(moveRoutine);
        //            isRunning = false;
        //        }
        //        moveRoutine = StartCoroutine(MoveVehicle());
        //    }
        //}
    }

    //private IEnumerator MoveVehicle()
    //{
    //    if (isRunning)
    //    {
    //        yield break;
    //    }
    //    isRunning = true;

    //    PlayerTurnSFX();

    //    float progress = 0f;
    //    float duration = 0.5f;

    //    var originPosition = transform.position;
    //    Quaternion originRotation = transform.rotation;

    //    while (progress <= 1f)
    //    {
    //        transform.SetPositionAndRotation(Vector3.Lerp(originPosition, TartgetPos, progress), Quaternion.Slerp(originRotation, TargetRot, progress));

    //        progress += Time.deltaTime / duration;
    //        //Debug.Log("Progress " + progress);
    //        yield return null;
    //    }
    //    //reset rotation
    //    duration = 0.2f;
    //    progress = 0f;
    //    while (progress <= 1f)
    //    {
    //        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, progress);
    //        progress += Time.deltaTime / duration;
    //        yield return null;
    //    }
    //    isRunning = false;
    //}
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
