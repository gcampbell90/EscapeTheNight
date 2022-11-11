using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimController : MonoBehaviour
{
    [SerializeField] Camera aimCamera;
    Camera mainCamera;
    void Awake()
    {
        mainCamera = Camera.main;
        Vector3 euler = transform.rotation.eulerAngles;
        X = euler.x;
        Y = euler.y;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (aimCamera.enabled)
        {
            aimCamera.enabled = false;
        }
    }


    float X, Y;
    [SerializeField] Vector2 sensitivity = new Vector2(1, 1);
    [SerializeField] Vector2 acceleration;
    Vector2 velocity;
    Vector2 rotation;

    [SerializeField] float maxVerticalAngleHorizon;
    [SerializeField] float maxHorizontalAngleHorizon;

    //Gun System
    [SerializeField] Transform pivot;
    [SerializeField] Transform pivot1;

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

    private void FollowMouse()
    {
        Vector2 targetVelocity = GetInput() * sensitivity;

        velocity = new Vector2(
            Mathf.MoveTowards(velocity.x, targetVelocity.x, acceleration.x * Time.deltaTime),
            Mathf.MoveTowards(velocity.y, targetVelocity.y, acceleration.y * Time.deltaTime));

        rotation += velocity * Time.deltaTime;

        rotation.y = ClampVerticalAngle(rotation.y);
        rotation.x = ClampHorizontalAngle(rotation.x);

        aimCamera.transform.eulerAngles = new Vector3(-rotation.y, rotation.x, 0);
        pivot.transform.eulerAngles = new Vector3(-rotation.y, rotation.x, 0);
        pivot1.transform.eulerAngles = new Vector3(-rotation.y, rotation.x, 0);
    }

    Vector2 GetInput()
    {
        Vector2 input = new Vector2(
            Input.GetAxis("Mouse X"),
            Input.GetAxis("Mouse Y")
            );

        return input;
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
        }else
        {
            pivot.transform.rotation = Quaternion.identity;
            pivot1.transform.rotation = Quaternion.identity;
        }

    }

}
