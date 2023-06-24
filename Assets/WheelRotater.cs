using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelRotater : MonoBehaviour
{
    [SerializeField]
    GameObject[] wheels;

    [SerializeField]
    private float rotateSpeed;

    private void Start()
    {
        StartCoroutine(RotateWheels());
    }

    private IEnumerator RotateWheels()
    {
        while (true)
        {
            foreach (var item in wheels)
            {
                item.transform.Rotate(Vector3.up, -rotateSpeed * Time.deltaTime);
            }
            yield return null;
        }
    }
}
