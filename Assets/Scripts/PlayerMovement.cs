using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerController playerController;

    Coroutine moveRoutine = null;

    //Movement
    bool dir;
    float rot;
    float pos;

    public Vector3 TartgetPos => SetTargetPosition();
    public Quaternion TargetRot => SetTargetRotation();

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {

        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            //Debug.Log("Key Pressed");
            if (Input.GetKeyDown(KeyCode.W))
            {

                if (isRunning) { StopCoroutine(moveRoutine); };
                playerController.BoostController.Booster();
            }

            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
            {
                //Dont allow turning if boosting
                if (playerController.BoostController.IsBoosting) return;

                if (Input.GetKeyDown(KeyCode.A))
                {
                    //Debug.Log("Turning Left");

                    dir = true;
                }
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    //Debug.Log("Turning Right");

                    dir = false;
                }
                else
                {
                    return;
                }

                if (isRunning && moveRoutine != null)
                {
                    StopCoroutine(moveRoutine);
                    isRunning = false;
                }
                moveRoutine = StartCoroutine(MoveVehicle());
            }
        }
    }
    bool isRunning = false;
    private IEnumerator MoveVehicle()
    {
        if (isRunning)
        {
            yield break;
        }
        isRunning = true;

        //PlayerTurnSFX();

        float progress = 0f;
        float duration = 0.5f;

        var originPosition = transform.position;
        Quaternion originRotation = transform.rotation;

        while (progress <= 1f)
        {
            transform.SetPositionAndRotation(Vector3.Lerp(originPosition, TartgetPos, progress), Quaternion.Slerp(originRotation, TargetRot, progress));

            progress += Time.deltaTime / duration;
            //Debug.Log("Progress " + progress);
            yield return null;
        }
        //reset rotation
        duration = 0.2f;
        progress = 0f;
        while (progress <= 1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, progress);
            progress += Time.deltaTime / duration;
            yield return null;
        }
        isRunning = false;
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
