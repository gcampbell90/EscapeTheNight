using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class AimController : MonoBehaviour
{
    [SerializeField] Camera aimCamera;
    Camera mainCamera;

    [SerializeField] Vector2 sensitivity = new Vector2(1, 1);
    [SerializeField] Vector2 acceleration;
    Vector2 velocity;
    Vector2 rotation;

    [SerializeField] float maxVerticalAngleHorizon;
    [SerializeField] float maxHorizontalAngleHorizon;

    //Gun System
    [SerializeField] Transform pivot;
    [SerializeField] Transform pivot1;

    [SerializeField] GameObject gun_UI, main_UI;

    LineRenderer lineRendererL;
    LineRenderer lineRendererR;
    void Awake()
    {
        mainCamera = Camera.main;
        Vector3 euler = transform.rotation.eulerAngles;

        lineRendererL = pivot.GetComponentInChildren<LineRenderer>();
        lineRendererR = pivot1.GetComponentInChildren<LineRenderer>();

        lineRendererL.enabled = false;
        lineRendererR.enabled = false;


    }

    // Start is called before the first frame update
    void Start()
    {
        if (aimCamera.enabled)
        {
            aimCamera.enabled = false;
            gun_UI.SetActive(false);
        }
    }

    float ClampVerticalAngle(float angle)
    {
        return Mathf.Clamp(angle, 1.5f, maxVerticalAngleHorizon);
    }
    float ClampHorizontalAngle(float angle)
    {
        return Mathf.Clamp(angle, -maxHorizontalAngleHorizon, maxHorizontalAngleHorizon);
    }

    //Vector2 GetClampedAngles(Vector2 angle)
    //{
    //    var newX = Mathf.Clamp(angle.x, -maxHorizontalAngleHorizon, maxHorizontalAngleHorizon);
    //    var newY = Mathf.Clamp(angle.x, -maxVerticalAngleHorizon, maxVerticalAngleHorizon);

    //    return new Vector2(newX, newY);
    //}

    //use unscaled time to avoid input being influenced by slowed timescale 
    float _timer = 0f;

    private void FollowMouse()
    {
        Vector2 targetVelocity = GetInput() * sensitivity;

        velocity = new Vector2(
            Mathf.MoveTowards(velocity.x, targetVelocity.x, acceleration.x * _timer),
            Mathf.MoveTowards(velocity.y, targetVelocity.y, acceleration.y * _timer));

        rotation += velocity * Time.deltaTime;

        rotation.y = ClampVerticalAngle(rotation.y);
        rotation.x = ClampHorizontalAngle(rotation.x);

        aimCamera.transform.eulerAngles = new Vector3(-rotation.y, rotation.x, 0);
        pivot.transform.eulerAngles = new Vector3(-rotation.y, rotation.x, 0);
        pivot1.transform.eulerAngles = new Vector3(-rotation.y, rotation.x, 0);

        _timer += Time.unscaledDeltaTime;
    }

    Vector2 GetInput()
    {
        Vector2 input = new Vector2(
            Input.GetAxis("Mouse X"),
            Input.GetAxis("Mouse Y")
            );

        return input;
    }
    bool isFiring;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            mainCamera.enabled = !mainCamera.enabled;
            aimCamera.enabled = !aimCamera.enabled;
        }
        if (mainCamera.enabled == false)
        {
            FollowMouse();
            if (!isFiring)
            {
                Debug.Log("SHould be called in update once(with if check)");
                SlowTime();
                ToggleUI(true);
            }
            isFiring = true;
        }
        else
        {
            if (!isFiring) return;
            Debug.Log("SHould be called in update once(with if check)");
            isFiring = false;
            pivot.transform.rotation = Quaternion.identity;
            pivot1.transform.rotation = Quaternion.identity;
            NormalTime();
            ToggleUI(false);

        }

        if (isFiring == true && Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartCoroutine(FireAtTarget());

        }
        else if (isFiring == false || Input.GetKeyUp(KeyCode.Mouse0))
        {
            Debug.Log("Mouse0 up");
            StopAllCoroutines();
        }
    }

    private IEnumerator FireAtTarget()
    {
        while (!Input.GetKeyUp(KeyCode.Mouse0))
        {
            var shootPositionL = aimCamera.transform.position;
            var shootPositionR = aimCamera.transform.position;

            var shootDirectionL = aimCamera.transform.TransformDirection(Vector3.forward);
            var shootDirectionR = aimCamera.transform.TransformDirection(Vector3.forward);
            //replacing with camera raycast
            //var shootPositionL = pivot.position;
            //var shootPositionR = pivot1.position;

            //var shootDirectionL = pivot.transform.TransformDirection(Vector3.forward);//might work
            //var shootDirectionR = pivot1.transform.TransformDirection(Vector3.forward);//might work

            RaycastHit hitL;
            RaycastHit hitR;


            lineRendererL.enabled = true;
            lineRendererR.enabled = true;

            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(shootPositionL, shootDirectionL, out hitL, 100))
            {
                Debug.DrawRay(shootPositionL, shootDirectionL * hitL.distance, Color.green);
                SpawnEffect(hitL, shootDirectionL);

                Debug.Log("L Did Hit " + hitL.collider);
                hitL.transform.SendMessage("HitByRay", SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                Debug.DrawRay(shootPositionL, shootDirectionL * 1000, Color.red);
                Debug.Log("L Did not Hit");
            }
            if (Physics.Raycast(shootPositionR, shootDirectionR, out hitR, 100))
            {
                Debug.DrawRay(shootPositionR, shootDirectionR * hitR.distance, Color.green);
                SpawnEffect(hitR, shootDirectionR);

                Debug.Log("R Did Hit " + hitR.collider);
                hitR.transform.SendMessage("HitByRay", SendMessageOptions.DontRequireReceiver);

            }
            else
            {
                Debug.DrawRay(shootPositionR, shootDirectionR * 1000, Color.red);
                Debug.Log("R Did not Hit");
            }

            yield return new WaitForEndOfFrame();
            lineRendererL.enabled = false;
            lineRendererR.enabled = false;
            yield return new WaitForSeconds(0.05f);
        }

    }

    [SerializeField] GameObject sprite;
    private void SpawnEffect(RaycastHit hit, Vector3 dir)
    {
        Debug.Log("Spawn Gun hit Sprite");

        var spriteTmp = Instantiate(sprite, hit.point, Quaternion.Euler(dir), hit.transform);
        //spriteTmp.transform.SetParent(point.transform, true);
        spriteTmp.SetActive(true);
    }

    private void ToggleUI(bool isOn)
    {
        main_UI.SetActive(!isOn);
        gun_UI.SetActive(isOn);
    }
    private void SlowTime()
    {
        Time.timeScale = 0.25f;
    }

    private void NormalTime()
    {
        Time.timeScale = 1;
    }
}
