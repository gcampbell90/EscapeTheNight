using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static CarPositionChanger;
using Random = System.Random;

public class CarPositionChanger : MonoBehaviour
{
    public delegate void OnChangePosition();
    public static OnChangePosition onChangePosition;

    int[] preDefinedPosX = new int[] { -5, 0, 5 };
    Random random = new Random();

    private void OnEnable()
    {
        onChangePosition += ChangePosition;
    }

    private void OnDisable()
    {
        onChangePosition -= ChangePosition;
    }
    private void ChangePosition()
    {

        foreach(Transform child in transform)
        {
            int randomIndex = random.Next(0, preDefinedPosX.Length);
            int selectedNumber = preDefinedPosX[randomIndex];

            child.transform.position = new Vector3(selectedNumber, child.transform.position.y, child.transform.position.z);
        }
    }
}
