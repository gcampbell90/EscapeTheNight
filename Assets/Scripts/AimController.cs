using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Mathematics;
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

    //Gun System 1
    [SerializeField] Transform gun1_Pivot;
    [SerializeField] Transform gun1_Pivot1;

    //Gun System 2
    [SerializeField] Transform gun2_Pivot;
    [SerializeField] Transform gun2_Pivot1;

    [SerializeField] GameObject gun_UI, main_UI;

    //LineRenderer lineRendererL;
    //LineRenderer lineRendererR;

    [SerializeField] GameObject sprite;

    [SerializeField] LineRenderer rendL;
    [SerializeField] LineRenderer rendR;


    [SerializeField] ParticleSystem MuzzleFlashL;
    [SerializeField] ParticleSystem MuzzleFlashR;
    [SerializeField] GameObject ImpactEffect;

    [SerializeField] Transform target;

    //use unscaled time to avoid input being influenced by slowed timescale 
    float timer = 0f;

    bool isFiring;

    void Awake()
    {
        mainCamera = Camera.main;
        Vector3 euler = transform.rotation.eulerAngles;

        //lineRendererL = rendL;
        //lineRendererR = rendR;

        //lineRendererL.enabled = false;
        //lineRendererR.enabled = false;
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
                SlowTime();
                ToggleUI(true);
            }
            isFiring = true;
        }
        else
        {
            if (!isFiring) return;
            isFiring = false;
            gun1_Pivot.transform.rotation = Quaternion.identity;
            gun1_Pivot1.transform.rotation = Quaternion.identity;
            gun2_Pivot.transform.rotation = Quaternion.identity;
            gun2_Pivot1.transform.rotation = Quaternion.identity;
            NormalTime();
            ToggleUI(false);

        }

        if (isFiring == true && Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartCoroutine(FireAtTarget());

        }
        else if (isFiring == false || Input.GetKeyUp(KeyCode.Mouse0))
        {
            //Debug.Log("Mouse0 up");
            StopAllCoroutines();
        }
    }

    private void FollowMouse()
    {
        Vector2 targetVelocity = GetInput() * sensitivity;

        velocity = new Vector2(
            Mathf.MoveTowards(velocity.x, targetVelocity.x, acceleration.x * timer),
            Mathf.MoveTowards(velocity.y, targetVelocity.y, acceleration.y * timer));

        rotation += velocity * Time.deltaTime;

        rotation.y = ClampVerticalAngle(rotation.y);
        rotation.x = ClampHorizontalAngle(rotation.x);

        aimCamera.transform.eulerAngles = new Vector3(-rotation.y, rotation.x, 0);

        gun1_Pivot.transform.rotation = Quaternion.Euler(-rotation.y, rotation.x, 0);
        gun1_Pivot1.transform.rotation = Quaternion.Euler(-rotation.y, rotation.x, 0);

        gun2_Pivot.transform.rotation = Quaternion.Euler(-rotation.y, rotation.x, 0);
        gun2_Pivot1.transform.rotation = Quaternion.Euler(-rotation.y, rotation.x, 0);

        //pivot.transform.eulerAngles = new Vector3(-rotation.y, rotation.x, 0);
        //pivot1.transform.eulerAngles = new Vector3(-rotation.y, rotation.x, 0);

        timer += Time.unscaledDeltaTime;
    }

    private IEnumerator FireAtTarget()
    {
        while (!Input.GetKeyUp(KeyCode.Mouse0))
        {
            var shootPositionL = aimCamera.transform.position;
            var shootPositionR = aimCamera.transform.position;

            var shootDirectionL = aimCamera.transform.TransformDirection(Vector3.forward);
            var shootDirectionR = aimCamera.transform.TransformDirection(Vector3.forward);

            MuzzleFlashL.Play();
            MuzzleFlashR.Play();

            //replacing with camera raycast
            //var shootPositionL = pivot.position;
            //var shootPositionR = pivot1.position;

            //var shootDirectionL = pivot.transform.TransformDirection(Vector3.forward);//might work
            //var shootDirectionR = pivot1.transform.TransformDirection(Vector3.forward);//might work

            RaycastHit hitL;
            RaycastHit hitR;


            //lineRendererL.enabled = true;
            //lineRendererR.enabled = true;

            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(shootPositionL, shootDirectionL, out hitL, 100))
            {
                Debug.DrawRay(shootPositionL, shootDirectionL * hitL.distance, Color.green);
                SpawnEffect(hitL, shootDirectionL);

                //Debug.Log("L Did Hit " + hitL.collider);
                hitL.transform.SendMessage("HitByRay", SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                Debug.DrawRay(shootPositionL, shootDirectionL * 1000, Color.red);
                //Debug.Log("L Did not Hit");
            }
            if (Physics.Raycast(shootPositionR, shootDirectionR, out hitR, 100))
            {
                Debug.DrawRay(shootPositionR, shootDirectionR * hitR.distance, Color.green);
                SpawnEffect(hitR, shootDirectionR);

                //Debug.Log("R Did Hit " + hitR.collider);
                hitR.transform.SendMessage("HitByRay", SendMessageOptions.DontRequireReceiver);

            }
            else
            {
                Debug.DrawRay(shootPositionR, shootDirectionR * 1000, Color.red);
                //Debug.Log("R Did not Hit");
            }

            yield return new WaitForEndOfFrame();
            //lineRendererL.enabled = false;
            //lineRendererR.enabled = false;
            yield return new WaitForSeconds(0.05f);
        }

    }
    private void SpawnEffect(RaycastHit hit, Vector3 dir)
    {
        //Debug.Log("Spawn Gun hit Sprite");

        var spriteTmp = Instantiate(sprite, hit.point, Quaternion.identity, hit.transform);
        //spriteTmp.transform.SetParent(point.transform, true);
        spriteTmp.SetActive(true);

        GameObject impactGO = Instantiate(ImpactEffect, hit.point, Quaternion.Euler(dir));
        Destroy(impactGO, 2f);
    }
    private void ToggleUI(bool isOn)
    {
        main_UI.SetActive(!isOn);
        gun_UI.SetActive(isOn);
    }
    private void NormalTime()
    {
        Time.timeScale = 1;
    }
    private void SlowTime()
    {
        Time.timeScale = 0.25f;
    }
    Vector2 GetInput()
    {
        Vector2 input = new Vector2(
            Input.GetAxis("Mouse X"),
            Input.GetAxis("Mouse Y")
            );

        return input;
    }
    float ClampVerticalAngle(float angle)
    {
        return Mathf.Clamp(angle, 1.5f, maxVerticalAngleHorizon);
    }
    float ClampHorizontalAngle(float angle)
    {
        return Mathf.Clamp(angle, -maxHorizontalAngleHorizon, maxHorizontalAngleHorizon);
    }

}
